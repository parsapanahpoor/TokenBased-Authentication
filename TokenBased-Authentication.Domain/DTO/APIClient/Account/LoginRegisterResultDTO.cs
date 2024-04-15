namespace TokenBased_Authentication.Domain.DTO.APIClient.Account;

public record LoginRegisterResultDTO
{
    #region properties

    public bool IsSuccess { get; set; }

    public string Message { get; set; }

    public LoginDataDto Data { get; set; }

    #endregion
}

public record LoginDataDto
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }
}