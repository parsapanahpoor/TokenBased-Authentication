using Microsoft.EntityFrameworkCore;
using TokenBased_Authentication.Domain.Entities.Role;
using TokenBased_Authentication.Domain.IRepositories.Role;

namespace TokenBased_Authentication.Infrastructure.Repositories.Role;

public class RoleCommandRepository : CommandGenericRepository<Domain.Entities.Role.Role>, IRoleCommandRepository
{
    #region Ctor

    private readonly TokenBased_AuthenticationDbContext _context;

    public RoleCommandRepository(TokenBased_AuthenticationDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion

    #region Site Side

    public async Task RemoveUserRolesByUserId(ulong userId, CancellationToken cancellationToken)
    {
        var userSelectedRoles = await _context.UserRoles
                                              .AsNoTracking()
                                              .Where(p => p.UserId == userId)
                                              .ToListAsync();

        _context.UserRoles.RemoveRange(userSelectedRoles);
    }

    public async Task AddUserSelectedRole(UserRole userRole, CancellationToken cancellationToken)
    {
        await _context.UserRoles.AddAsync(userRole);
    }

    public async Task AddPermissionToRole(RolePermission rolePermission)
    {
        await _context.RolePermissions.AddAsync(rolePermission);
    }

    public async Task RemoveRolePermissions(ulong roleId, List<ulong> rolePermissions, CancellationToken cancellation)
    {
        //Get Role Permissions
        foreach (var permissionId in rolePermissions)
        {
            var permission = await _context.RolePermissions
                                           .FirstOrDefaultAsync(p => !p.IsDelete &&
                                                                p.RoleId == roleId &&
                                                                p.PermissionId == permissionId);
            if (permission != null)
            {
                _context.RolePermissions.Remove(permission);
            }
        }
    }

    #endregion
}
