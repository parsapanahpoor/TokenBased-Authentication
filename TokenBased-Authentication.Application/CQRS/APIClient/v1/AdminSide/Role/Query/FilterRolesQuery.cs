using TokenBased_Authentication.Domain.DTO.AdminSide.Role;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.Role.Query;

public class FilterRolesQuery : IRequest<FilterRolesDTO>
{
    #region properties

    public string? RoleTitle { get; set; }

    #endregion
}
