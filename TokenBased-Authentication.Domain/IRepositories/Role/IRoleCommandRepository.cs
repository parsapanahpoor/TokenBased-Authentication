
using TokenBased_Authentication.Domain.Entities.Role;

namespace TokenBased_Authentication.Domain.IRepositories.Role;

public interface IRoleCommandRepository
{
    #region Site Side

    Task RemoveUserRolesByUserId(ulong userId, CancellationToken cancellationToken);

    Task AddUserSelectedRole(UserRole userRole, CancellationToken cancellationToken);

    Task AddAsync(Domain.Entities.Role.Role role, CancellationToken cancellationToken);

    void Update(Domain.Entities.Role.Role role);

    Task AddPermissionToRole(RolePermission rolePermission);

    Task RemoveRolePermissions(ulong roleId, List<ulong> rolePermissions, CancellationToken cancellation);

    #endregion
}
