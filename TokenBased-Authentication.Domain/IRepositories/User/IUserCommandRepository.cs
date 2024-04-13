using TokenBased_Authentication.Domain.Entities.Account;

namespace TokenBased_Authentication.Domain.IRepositories.User;

public interface IUserCommandRepository
{
    #region General Methods

    Task AddAsync(Domain.Entities.Account.User user, CancellationToken cancellationToken);

    void Update(Domain.Entities.Account.User user);

    #endregion
}
