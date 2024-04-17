using TokenBased_Authentication.Application.Utilities.Security;
using TokenBased_Authentication.Domain.Entities.Account;
using TokenBased_Authentication.Domain.IRepositories.User;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Query.FindRefreshToken;

public record FindRefreshTokenQueryHandler : IRequestHandler<FindRefreshTokenQuery, FindRefreshTokenQueryResult>
{
    #region Ctor

    private readonly IUserQueryRepository _userQueryRepository;

    public FindRefreshTokenQueryHandler(IUserQueryRepository userQueryRepository)
    {
        _userQueryRepository = userQueryRepository;
    }

    #endregion

    public async Task<FindRefreshTokenQueryResult?> Handle(FindRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        string RefreshTokenHash = request.RefreshToken.Getsha256Hash();

        //Get UserToken By Refresh Token
        var refreshToken = await _userQueryRepository.Get_UserToken_ByRefreshToken(request.RefreshToken);
        if (refreshToken == null) return null;

        return new FindRefreshTokenQueryResult()
        {
            TokenExpireTime  = refreshToken.RefreshTokenExpireTime,
            UserId = refreshToken.UserId,
            RefreshTokenId = refreshToken.Id
        };
    }
}
