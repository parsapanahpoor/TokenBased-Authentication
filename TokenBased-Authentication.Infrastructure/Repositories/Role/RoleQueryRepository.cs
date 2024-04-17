using TokenBased_Authentication.Application.StaticTools;
using TokenBased_Authentication.Domain.DTO.AdminSide.Role;
using TokenBased_Authentication.Domain.DTO.Common;
using TokenBased_Authentication.Domain.IRepositories.Role;
using Microsoft.EntityFrameworkCore;
namespace TokenBased_Authentication.Infrastructure.Repositories.User;

public class RoleQueryRepository : QueryGenericRepository<Domain.Entities.Role.Role>, IRoleQueryRepository
{
    #region Ctor

    private readonly TokenBased_AuthenticationDbContext _context;

    public RoleQueryRepository(TokenBased_AuthenticationDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion

    #region General Methods 



    #endregion

    #region Admin Side 

    public async Task<bool> HasUserPermission(ulong userId, string permissionName)
    {
        // get user
        var user = await _context.Users
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(s => s.Id == userId && !s.IsDelete);

        // check user exists
        if (user == null) return false;

        // admin user access any where
        if (user.IsAdmin) return true;

        // get permission from permission list
        var permission = PermissionsList.Permissions.FirstOrDefault(s => s.PermissionUniqueName == permissionName);

        // check permission exists
        if (permission == null) return false;

        // get user roles
        var userRoles = await _context.UserRoles
                                      .AsNoTracking()
                                      .Where(s => s.UserId == userId &&
                                             !s.IsDelete)
                                       .ToListAsync();

        // check user has any roles
        if (!userRoles.Any()) return false;

        // get user role Ids list
        var userRoleIds = userRoles.Select(s => s.RoleId).ToList();

        // check user has permission
        var result = await _context.RolePermissions.AnyAsync(s =>
            s.PermissionId == permission.Id && userRoleIds.Contains(s.RoleId) && !s.IsDelete);

        return result;
    }

    public async Task<List<ulong>> GetUserSelectedRoleIdByUserId(ulong userId,
                                                                 CancellationToken cancellation)
    {
        return await _context.UserRoles
                             .AsNoTracking()
                             .Where(s => !s.IsDelete &&
                                    s.UserId == userId)
                             .Select(s => s.RoleId)
                             .ToListAsync();
    }

    public async Task<List<SelectListViewModel>> GetSelectRolesList(CancellationToken cancellation)
    {
        return await _context.Roles
                             .AsNoTracking()
                             .Where(s => !s.IsDelete)
                             .Select(s => new SelectListViewModel
                             {
                                 Id = s.Id,
                                 Title = s.Title
                             })
                             .ToListAsync();
    }

    public async Task<FilterRolesDTO> FilterRoles(FilterRolesDTO filter, CancellationToken cancellation)
    {
        var query = _context.Roles
                           .AsNoTracking()
                           .Where(p=> !p.IsDelete)
                           .OrderByDescending(p =>  p.CreateDate)
                           .AsQueryable();

        #region filter

        if ((!string.IsNullOrEmpty(filter.RoleTitle)))
        {
            query = query.Where(u => u.Title.Contains(filter.RoleTitle));
        }

        #endregion

        #region paging

        await filter.Paging(query);

        #endregion

        return filter;
    }

    public async Task<bool> IsExistAnyRoleByRoleUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return await _context.Roles
                             .AsNoTracking()
                             .AnyAsync(p => !p.IsDelete &&
                                       p.Title == title);
    }

    public async Task<Domain.Entities.Role.Role?> GetRoleByUniqueTitle(string title, CancellationToken cancellation)
    {
        return await _context.Roles
                             .AsNoTracking()
                             .FirstOrDefaultAsync(p => !p.IsDelete &&
                                                  p.RoleUniqueName == title);
    }

    public async Task<List<ulong>> GetRolePermissionsIdByRoleId(ulong roleId, CancellationToken cancellationToken)
    {
        return await _context.RolePermissions
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.RoleId == roleId)
                             .Select(p => p.PermissionId)
                             .ToListAsync();
    }

    public async Task<bool> IsUser_Admin(ulong userId, 
                                         CancellationToken cancellationToken)
    {
        //Check That is user super admin 
        var isUserSuperAdmin = await IsUserIsSuperAdmin(userId, cancellationToken);
        if (isUserSuperAdmin) return true;

        //Get User Roles
        var userRolesName = await GetListOf_UserUniqueRolesName(userId, cancellationToken);
        if (userRolesName != null && userRolesName.Any() && userRolesName.Contains("Admin")) return true;

        return false;
    }

    public async Task<bool> IsUserIsSuperAdmin(ulong userId, 
                                               CancellationToken cancellationToken)
    {
        return await _context.Users
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.Id == userId)
                             .Select(p => p.IsAdmin)
                             .FirstOrDefaultAsync();
    }

    public async Task<List<string?>> GetListOf_UserUniqueRolesName(ulong userId,
                                                                   CancellationToken cancellationToken)
    {
        //Get User Selected Role Ids
        var roleIds = await _context.UserRoles
                                    .AsNoTracking()
                                    .Where(p => !p.IsDelete && p.Id == userId)
                                    .Select(p => p.RoleId)
                                    .ToListAsync();

        List<string?> roleNames = new List<string?>();

        foreach (var roleId in roleIds)
        {
            roleNames.Add(await _context.Roles
                                        .AsNoTracking()
                                        .Where(p => !p.IsDelete &&
                                               p.Id == roleId)
                                        .Select(p => p.RoleUniqueName)
                                        .FirstOrDefaultAsync());
        }

        return roleNames;
    }

    #endregion
}
