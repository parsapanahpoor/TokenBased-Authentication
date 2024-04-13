﻿using TokenBased_Authentication.Domain.IRepositories.User;
using Microsoft.EntityFrameworkCore;
namespace TokenBased_Authentication.Infrastructure.Repositories.User;

public class UserQueryRepository : QueryGenericRepository<Domain.Entities.Account.User>, IUserQueryRepository
{
    #region Ctor

    private readonly TokenBased_AuthenticationDbContext _context;

    public UserQueryRepository(TokenBased_AuthenticationDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion

    #region General Methods

    public async Task<bool> IsMobileExist(string mobile, CancellationToken cancellation)
    {
        return await _context.Users
                             .AsNoTracking()
                             .AnyAsync(p => !p.IsDelete &&
                                       p.Mobile == mobile);
    }

    public async Task<bool> IsUserActive(string mobile, CancellationToken cancellation)
    {
        return await _context.Users 
                             .AsNoTracking() 
                             .AnyAsync(p => !p.IsDelete &&
                                       p.Mobile == mobile &&
                                       p.IsActive);
    }

    public async Task<bool> IsPasswordValid(string mobile, string password, CancellationToken cancellation)
    {
        return await _context.Users
                             .AsNoTracking()
                             .AnyAsync(p => !p.IsDelete &&
                                       p.Mobile == mobile &&
                                       p.Password == password);
    }

    public async Task<Domain.Entities.Account.User?> GetUserByMobile(string mobile , CancellationToken cancellation)
    {
        return await _context.Users
                             .AsNoTracking()
                             .FirstOrDefaultAsync(p => !p.IsDelete &&
                                                  p.Mobile == mobile);
    }

    #endregion

    #region Site Side

 

    #endregion
}