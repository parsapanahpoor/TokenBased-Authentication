using TokenBased_Authentication.Domain.Entities.Account;
using TokenBased_Authentication.Domain.IRepositories.User;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Query.CreateToken;

public record CreateTokenQueryHandler : IRequestHandler<CreateTokenQuery, User>
{
    #region Ctor

    private readonly IUserQueryRepository _userQueryRepository;

    public CreateTokenQueryHandler(IUserQueryRepository userQueryRepository)
    {
        _userQueryRepository = userQueryRepository;
    }

    #endregion

    public async Task<User> Handle(CreateTokenQuery request, CancellationToken cancellationToken)
    {
        return await _userQueryRepository.GetByIdAsync(cancellationToken , request.UserId);
    }
}
