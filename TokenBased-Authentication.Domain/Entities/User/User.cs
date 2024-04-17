namespace TokenBased_Authentication.Domain.Entities.Account;

public sealed class User : BaseEntities<ulong>
{
    #region properties

    public string Username { get; set; }

    public string Mobile { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsActive { get; set; }

    #endregion
}
