using TokenBased_Authentication.Domain.Entities.Account;

namespace TokenBased_Authentication.Domain.IRepositories.User;

public interface IUserQueryRepository
{
    #region General Methods

    Task<SmsCode?> GetSMSCode_ByMobileAndCode(string mobile, string code);

    Task<bool> IsMobileExist(string mobile, CancellationToken cancellation);

    Task<bool> IsUserActive(string mobile , CancellationToken cancellation);

    Task<Domain.Entities.Account.User?> GetUserByMobile(string mobile, CancellationToken cancellation);

    Task<Domain.Entities.Account.User> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    #endregion

    #region Site Side


    #endregion
}
