using TokenBased_Authentication.Domain.DTO.Common;

namespace TokenBased_Authentication.Domain.DTO.AdminSide.Role;

public class FilterRolesDTO : BasePaging<Entities.Role.Role>
{
    #region properties

    public string? RoleTitle { get; set; }

    #endregion
}
