using TokenBased_Authentication.Domain.DTO.APIClient.Account;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Query.Login;

public record LoginQuery : IRequest<SMSCodeInsertedResultDTO>
{
    #region properties

    public string PhoneNumber { get; set; }

    public string SmsCode { get; set; }

    #endregion
}
