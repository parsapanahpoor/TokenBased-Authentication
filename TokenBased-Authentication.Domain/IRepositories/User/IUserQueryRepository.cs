namespace TokenBased_Authentication.Domain.IRepositories.User;

public interface IUserQueryRepository
{
    #region General Methods

    Task<bool> IsMobileExist(string mobile, CancellationToken cancellation);

    Task<bool> IsUserActive(string mobile , CancellationToken cancellation);

    Task<bool> IsPasswordValid(string mobile, string password, CancellationToken cancellation);

    Task<Domain.Entities.Account.User?> GetUserByMobile(string mobile, CancellationToken cancellation);

    Task<Domain.Entities.Account.User> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    #endregion

    #region Site Side


    #endregion
}
