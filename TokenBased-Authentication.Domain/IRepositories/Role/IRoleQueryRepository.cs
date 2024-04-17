using TokenBased_Authentication.Domain.DTO.AdminSide.Role;
using TokenBased_Authentication.Domain.DTO.Common;

namespace TokenBased_Authentication.Domain.IRepositories.Role;

public interface IRoleQueryRepository
{
    #region General Methods



    #endregion

    #region Admin Side 

    Task<bool> HasUserPermission(ulong userId, string permissionName);

    Task<List<ulong>> GetUserSelectedRoleIdByUserId(ulong userId,
                                                                 CancellationToken cancellation);

    Task<Domain.Entities.Role.Role> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    Task<List<SelectListViewModel>> GetSelectRolesList(CancellationToken cancellation);

    Task<FilterRolesDTO> FilterRoles(FilterRolesDTO filter, CancellationToken cancellation);

    Task<bool> IsExistAnyRoleByRoleUniqueTitle(string title, CancellationToken cancellationToken);

    Task<Domain.Entities.Role.Role?> GetRoleByUniqueTitle(string title, CancellationToken cancellation);

    Task<List<ulong>> GetRolePermissionsIdByRoleId(ulong roleId, CancellationToken cancellationToken);

    Task<bool> IsUser_Admin(ulong userId,
                            CancellationToken cancellationToken);

    #endregion
}
