using TokenBased_Authentication.Application.Common.IUnitOfWork;
using TokenBased_Authentication.Application.Extensions;
using TokenBased_Authentication.Application.Generators;
using TokenBased_Authentication.Application.Security;
using TokenBased_Authentication.Application.StaticTools;
using TokenBased_Authentication.Application.Utilities.Security;
using TokenBased_Authentication.Domain.DTO.AdminSide.User;
using TokenBased_Authentication.Domain.Entities.Role;
using TokenBased_Authentication.Domain.IRepositories.Role;
using TokenBased_Authentication.Domain.IRepositories.User;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.User.Command.EditUser;

public record EditUserAdminSideCommandHandler : IRequestHandler<EditUserAdminSideCommand, EditUserResult>
{
    #region Ctor

    private readonly IUserCommandRepository _userCommandRepository;
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IRoleCommandRepository _roleCommandRepository;
    private readonly IRoleQueryRepository _roleQueryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditUserAdminSideCommandHandler(IUserCommandRepository userCommandRepository,
                                  IUserQueryRepository userQueryRepository,
                                  IRoleCommandRepository roleCommandRepository,
                                  IRoleQueryRepository roleQueryRepository,
                                  IUnitOfWork unitOfWork)
    {
        _userCommandRepository = userCommandRepository;
        _userQueryRepository = userQueryRepository;
        _roleCommandRepository = roleCommandRepository;
        _roleQueryRepository = roleQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<EditUserResult> Handle(EditUserAdminSideCommand request, CancellationToken cancellationToken)
    {
        //Get User By Id 
        var userOldInfos = await _userQueryRepository.GetByIdAsync(cancellationToken, request.Id);
        if (userOldInfos == null) return EditUserResult.Error;

        //Checkind incomin mobile 
        if (await _userQueryRepository.IsMobileExist(request.Mobile, cancellationToken) && request.Mobile != userOldInfos.Mobile)
        {
            return EditUserResult.DuplicateMobileNumber;
        }

        if (userOldInfos != null)
        {
            userOldInfos.Username = request.Username;
            userOldInfos.IsActive = request.IsActive;
            userOldInfos.UpdateDate = DateTime.Now;

            #region User Avatar

            if (request.Avatar != null && request.Avatar.IsImage())
            {
                if (!string.IsNullOrEmpty(userOldInfos.Avatar))
                {
                    userOldInfos.Avatar.DeleteImage(FilePaths.UserAvatarPathServer, FilePaths.UserAvatarPathThumbServer);
                }

                var imageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(request.Avatar.FileName);
                request.Avatar.AddImageToServer(imageName, FilePaths.UserAvatarPathServer, 270, 270, FilePaths.UserAvatarPathThumbServer);
                userOldInfos.Avatar = imageName;
            }

            #endregion

            _userCommandRepository.Update(userOldInfos);

            #region Delete User Roles

            await _roleCommandRepository.RemoveUserRolesByUserId(request.Id, cancellationToken);

            #endregion

            #region Add User Roles

            if (request.UserRoles != null && request.UserRoles.Any())
            {
                foreach (var roleId in request.UserRoles)
                {
                    var userRole = new UserRole()
                    {
                        RoleId = roleId,
                        UserId = request.Id
                    };

                    await _roleCommandRepository.AddUserSelectedRole(userRole, cancellationToken);
                }
            }

            #endregion

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return EditUserResult.Success;
        }

        return EditUserResult.Error;
    }
}
