using TokenBased_Authentication.Application.Common.IUnitOfWork;
using TokenBased_Authentication.Domain.IRepositories.Role;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.Role.Command;
public record DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, bool>
{
    #region Ctor

    private readonly IRoleCommandRepository _roleCommandRepository;
    private readonly IRoleQueryRepository _roleQueryRepository;
    private readonly IUnitOfWork _unitOfWork;   

    public DeleteRoleCommandHandler(IRoleCommandRepository roleCommandRepository,
                                    IRoleQueryRepository roleQueryRepository , 
                                    IUnitOfWork unitOfWork)
    {
        _roleCommandRepository = roleCommandRepository;
        _roleQueryRepository = roleQueryRepository;
        _unitOfWork = unitOfWork;   
    }

    #endregion

    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        //Get Role By Id
        var role = await _roleQueryRepository.GetByIdAsync(cancellationToken, request.RoleId);
        if (role == null) return false;

        role.IsDelete = true;
        _roleCommandRepository.Update(role);

        // remove all permissions
        var rolePermissions = await _roleQueryRepository.GetRolePermissionsIdByRoleId(role.Id, cancellationToken);
        if (rolePermissions != null)
        {
            await _roleCommandRepository.RemoveRolePermissions(role.Id, rolePermissions, cancellationToken);
        }

            await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
