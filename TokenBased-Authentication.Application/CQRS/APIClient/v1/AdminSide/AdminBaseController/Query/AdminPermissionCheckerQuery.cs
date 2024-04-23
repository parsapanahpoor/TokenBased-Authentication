namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.AdminBaseController.Query;

public record AdminPermissionCheckerQuery : IRequest<bool>
{
    #region properties

    public ulong UserId { get; set; }

    public string? PermissionName { get; set; }

    #endregion
}
