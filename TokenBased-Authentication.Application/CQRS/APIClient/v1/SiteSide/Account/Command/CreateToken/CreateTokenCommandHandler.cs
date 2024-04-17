using System.Reflection;
using System.Security.Claims;
using System.Text;
using TokenBased_Authentication.Application.Common.IUnitOfWork;
using TokenBased_Authentication.Application.Utilities.Security;
using TokenBased_Authentication.Domain.DTO.APIClient.Account;
using TokenBased_Authentication.Domain.Entities.Account;
using TokenBased_Authentication.Domain.IRepositories.User;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Command.CreateToken;

public record CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, LoginDataDto>
{
    #region Ctor

    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IUserCommandRepository _userCommandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTokenCommandHandler(IUserQueryRepository userQueryRepository,
                                     IUserCommandRepository userCommandRepository,
                                     IUnitOfWork unitOfWork)
    {
        _userCommandRepository = userCommandRepository;
        _userQueryRepository = userQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<LoginDataDto> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
    {
        var userToken = new UserToken()
        {
            CreateDate = DateTime.Now,
            LastestSignInPlatformName = request.LastestSignInPlatformName,
            RefreshToken = request.RefreshToken.Getsha256Hash(),
            IsDelete = false,
            RefreshTokenExpireTime = request.RefreshTokenExpireTime,
            TokenExpireTime = request.TokenExpireTime,
            TokenHash = request.TokenHash.Getsha256Hash(),
            UserId = request.UserId
        };

        //Add To Data Base 
        await _userCommandRepository.Add_UserToken(userToken , cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginDataDto()
        {
            RefreshToken = request.RefreshToken,
            Token = request.TokenHash
        };
    }
}
