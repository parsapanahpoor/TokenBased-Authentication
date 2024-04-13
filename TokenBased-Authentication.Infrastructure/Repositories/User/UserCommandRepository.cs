using TokenBased_Authentication.Domain.IRepositories.User;
using TokenBased_Authentication.Infrastructure.Repositories;
namespace ClinicManagement.Infrastructure.Repositories.User;

public class UserCommandRepository : CommandGenericRepository<TokenBased_Authentication.Domain.Entities.Account.User>, IUserCommandRepository
{
    #region Ctor

    private readonly TokenBased_AuthenticationDbContext _context;

    public UserCommandRepository(TokenBased_AuthenticationDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion
}
