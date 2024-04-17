namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Query.FindRefreshToken;

public record FindRefreshTokenQueryResult
{
    #region properties

    public ulong UserId { get; set; }

    public ulong RefreshTokenId { get; set; }

    public DateTime TokenExpireTime { get; set; }

    #endregion
}
