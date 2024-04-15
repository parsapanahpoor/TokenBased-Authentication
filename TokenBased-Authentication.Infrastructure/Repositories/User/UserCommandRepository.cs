using TokenBased_Authentication.Domain.Entities.Account;
using TokenBased_Authentication.Domain.IRepositories.User;
using TokenBased_Authentication.Infrastructure.Repositories;
namespace TokenBased_Authentication.Infrastructure.Repositories.User;

public class UserCommandRepository : CommandGenericRepository<TokenBased_Authentication.Domain.Entities.Account.User>, IUserCommandRepository
{
    #region Ctor

    private readonly TokenBased_AuthenticationDbContext _context;

    public UserCommandRepository(TokenBased_AuthenticationDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion

    public void Update_SMSCode(SmsCode smsCode)
    {
        _context.SmsCodes.Update(smsCode);
    }

    public async Task Add_UserToken(UserToken userToken , CancellationToken cancellationToken)
    {
        await _context.UserTokens.AddAsync(userToken);
    }

    public async Task Add_SMSCode(SmsCode smsCode , CancellationToken cancellationToken)
    {
        await _context.SmsCodes.AddAsync(smsCode);
    }
}
