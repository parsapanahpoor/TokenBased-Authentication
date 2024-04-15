using TokenBased_Authentication.Domain.Entities.Account;

namespace TokenBased_Authentication.Domain.DTO.APIClient.Account;

public record SMSCodeInsertedResultDTO
{
    #region properties

    public User? User { get; set; }

    public bool IsSuccess { get; set; }

    public string Message { get; set; }

    #endregion
}
