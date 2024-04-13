using TokenBased_Authentication.Application.Common.IUnitOfWork;
using TokenBased_Authentication.Infrastructure.ApplicationDbContext;
namespace TokenBased_Authentication.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    #region Using

    private readonly TokenBased_AuthenticationDbContext _context;

    public UnitOfWork(TokenBased_AuthenticationDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Save Changes

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Dispose

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    #endregion
}
