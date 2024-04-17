using TokenBased_Authentication.Domain.Entities.Account;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Query.CreateToken;

public record CreateTokenQuery : IRequest<User>
{
    #region properties

    public ulong UserId { get; set; }

    #endregion
}
