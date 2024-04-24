using TokenBased_Authentication.Application.StaticTools;
using TokenBased_Authentication.Domain.DTO.AdminSide.User;
using TokenBased_Authentication.Domain.IRepositories.User;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.User.Query.UserDetailQuery;

public record UserDetailAdminSideQueryHandler : IRequestHandler<UserDetailAdminSideQuery, UserDetailAdminSideDTO>
{
    #region ctor

    private readonly IUserQueryRepository _userQueryRepository;

    public UserDetailAdminSideQueryHandler(IUserQueryRepository userQueryRepository)
    {
        _userQueryRepository = userQueryRepository;
    }

    #endregion

    public async Task<UserDetailAdminSideDTO?> Handle(UserDetailAdminSideQuery request, CancellationToken cancellationToken = default)
    {
        //Get User By Id 
        var user = await _userQueryRepository.GetByIdAsync(cancellationToken , request.UserId);
        if (user == null) return null;

        return new UserDetailAdminSideDTO()
        {
            UserId = user.Id,
            Avatar = $"{FilePaths.SiteAddress}{ FilePaths.UserAvatarPathThumb}/{user.Avatar}",
            IsActive = user.IsActive,
            IsAdmin = user.IsAdmin,
            Mobile = user.Mobile,
            Username = user.Username,
            UserRoles = await _userQueryRepository.ListOfUserRoles(request.UserId , cancellationToken)
        };
    }
}
