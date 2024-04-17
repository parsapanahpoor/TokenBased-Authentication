using TokenBased_Authentication.Domain.Entities.Account;
namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Query.FindRefreshToken;

public record FindRefreshTokenQuery : IRequest<FindRefreshTokenQueryResult>
{
    #region properties

    public string RefreshToken { get; set; }

    #endregion
}
