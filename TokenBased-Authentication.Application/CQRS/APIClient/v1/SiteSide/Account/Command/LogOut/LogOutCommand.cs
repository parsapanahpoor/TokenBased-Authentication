namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Command.LogOut;

public record LogOutCommand : IRequest<bool>
{
    #region properties

    public ulong UserId { get; set; }

    #endregion
}
