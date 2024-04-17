using TokenBased_Authentication.Domain.DTO.AdminSide.Role;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.Role.Query;

public record EditRoleQuery : IRequest<EditRoleDTO>
{
    public ulong RoleId { get; set; }
}
