
using TokenBased_Authentication.Application.Common.IUnitOfWork;
using TokenBased_Authentication.Domain.IRepositories.User;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Command.DeleteToken;

public record DeleteTokenCommandHandler : IRequestHandler<DeleteTokenCommand, bool>
{
    #region ctor

    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IUserCommandRepository _userCommandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTokenCommandHandler(IUserQueryRepository userQueryRepository ,
                                     IUserCommandRepository userCommandRepository ,
                                     IUnitOfWork unitOfWork)
    {
        _userCommandRepository = userCommandRepository;
        _userQueryRepository = userQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(DeleteTokenCommand request, CancellationToken cancellationToken)
    {
        //Get User Token By Id 
        var userToken = await _userQueryRepository.GetUserToken_ByUserTokenId(request.RefreshTokenId , cancellationToken);
        if (userToken == null) return false;

        //Delete User Token
        _userCommandRepository.Delete_UserToken(userToken);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
