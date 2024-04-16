
using TokenBased_Authentication.Application.Common.IUnitOfWork;
using TokenBased_Authentication.Domain.IRepositories.User;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Command.LogOut;

public record LogOutCommandHandler : IRequestHandler<LogOutCommand, bool>
{
    #region Ctor

    private readonly IUserCommandRepository _userCommandRepository;
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogOutCommandHandler(IUserCommandRepository userCommandRepository ,
                                IUserQueryRepository userQueryRepository ,
                                IUnitOfWork unitOfWork)
    {
        _userCommandRepository = userCommandRepository;
        _userQueryRepository = userQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(LogOutCommand request, CancellationToken cancellationToken)
    {
        //Get Token
        var userTokens = await _userQueryRepository.GetList_UserToken_ByUserId(request.UserId , cancellationToken);
        if (userTokens == null) return false;

        //Delete User Tokens
        _userCommandRepository.DeleteRange_UserTokens(userTokens);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
