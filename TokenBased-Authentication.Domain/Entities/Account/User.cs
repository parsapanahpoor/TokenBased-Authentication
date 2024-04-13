namespace TokenBased_Authentication.Domain.Entities.Account;

public sealed class User : BaseEntities<ulong>
{
    #region properties

    public string Username { get; set; }

    public string Mobile { get; set; }

    public string Password { get; set; }

    public bool IsActive { get; set; }

    #endregion
}
