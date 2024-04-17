namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.Role.Command;

public record DeleteRoleCommand : IRequest<bool>
{
    public ulong RoleId { get; set; }
}
