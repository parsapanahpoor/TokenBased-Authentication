using TokenBased_Authentication.Application.Common.IUnitOfWork;
using TokenBased_Authentication.Domain.DTO.APIClient.Account;
using TokenBased_Authentication.Domain.IRepositories.User;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Query.Login;

public record LoginQueryHandler : IRequestHandler<LoginQuery, SMSCodeInsertedResultDTO>
{
    #region Ctor

    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IUserCommandRepository _userCommandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LoginQueryHandler(IUserQueryRepository userQueryRepository,
                             IUserCommandRepository userCommandRepository,
                             IUnitOfWork unitOfWork)
    {
        _userCommandRepository = userCommandRepository;
        _userQueryRepository = userQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<SMSCodeInsertedResultDTO> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        //Get SMS Code Record By Mobile and Code 
        var smsCode = await _userQueryRepository.GetSMSCode_ByMobileAndCode(request.PhoneNumber, request.SmsCode);

        //Sms Code was not found
        if (smsCode == null) return new SMSCodeInsertedResultDTO { IsSuccess = false, Message = "کد وارد شده صحیح نیست!", };

        //Token was used and count of usage was completed
        if (smsCode.Used == true) return new SMSCodeInsertedResultDTO { IsSuccess = false, Message = "کدفعال سازی منقضی شده است." };

        //Update SMS Code Usage
        smsCode.RequestCount++;
        smsCode.Used = true;

        _userCommandRepository.Update_SMSCode(smsCode);
        await _unitOfWork.SaveChangesAsync();

        //Get User By Mobile Number
        var user = await _userQueryRepository.GetUserByMobile(request.PhoneNumber, cancellationToken);

        //If User is exist then login and return user 
        if (user != null) return new SMSCodeInsertedResultDTO { IsSuccess = true, User = user };

        //If user is not exist , register this user then return new registered user 
        if (user == null)
        {
            user = new Domain.Entities.Account.User()
            {
                CreateDate = DateTime.Now,
                IsActive = true,
                IsDelete = false,
                Mobile = request.PhoneNumber,
                Username = request.PhoneNumber,
            };

            //Add new user 
            await _userCommandRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            return new SMSCodeInsertedResultDTO { IsSuccess = true, User = user, };
        }

        return new SMSCodeInsertedResultDTO { IsSuccess = false, Message = "کد وارد شده صحیح نیست!", };
    }
}
