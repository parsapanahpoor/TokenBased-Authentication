using TokenBased_Authentication.Application.Common.IUnitOfWork;
using TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Command.SendSMSCode;
using TokenBased_Authentication.Domain.Entities.Account;
using TokenBased_Authentication.Domain.IRepositories.User;
namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Command.SendSMSCodel;

public record SendSMSCodeCommandHandler : IRequestHandler<SendSMSCodeCommand, string>
{
    #region Ctor

    private readonly IUserCommandRepository _userCommandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SendSMSCodeCommandHandler(IUserCommandRepository userCommandRepository, 
                                     IUnitOfWork unitOfWork)
    {
        _userCommandRepository = userCommandRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<string> Handle(SendSMSCodeCommand request, CancellationToken cancellationToken)
    {
        //Initial a random number
        Random random = new Random();
        string code = random.Next(1000, 9999).ToString();

        SmsCode smsCode = new SmsCode()
        {
            Code = code,
            InsertDate = DateTime.Now,
            PhoneNumber = request.PhoneNumber,
            RequestCount = 0,
            Used = false,
        };

        //Add to the data base
        await _userCommandRepository.Add_SMSCode(smsCode , cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        //Send SMS 
        //....

        return smsCode.Code;
    }
}
