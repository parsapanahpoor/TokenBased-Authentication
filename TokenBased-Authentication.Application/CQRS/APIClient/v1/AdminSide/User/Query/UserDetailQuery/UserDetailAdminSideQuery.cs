using TokenBased_Authentication.Domain.DTO.AdminSide.User;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.User.Query.UserDetailQuery;

public record UserDetailAdminSideQuery : IRequest<UserDetailAdminSideDTO>
{
    #region proeprties

    public ulong UserId { get; set; }

    #endregion
}
