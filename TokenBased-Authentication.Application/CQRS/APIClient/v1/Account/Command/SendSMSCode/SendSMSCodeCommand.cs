namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Command.SendSMSCode;

public record SendSMSCodeCommand : IRequest<string>
{
    public string PhoneNumber { get; set; }
}
