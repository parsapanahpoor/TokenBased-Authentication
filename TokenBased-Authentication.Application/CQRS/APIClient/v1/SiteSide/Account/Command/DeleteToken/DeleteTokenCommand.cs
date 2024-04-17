namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Command.DeleteToken;

public record DeleteTokenCommand : IRequest<bool>
{
    #region properties

    public ulong RefreshTokenId { get; set; }

    #endregion
}
