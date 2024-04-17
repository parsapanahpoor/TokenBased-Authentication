namespace TokenBased_Authentication.Domain.Entities.Role;

public class Role : BaseEntities<ulong>
{
    #region Properties

    public string  Title { get; set; }

    public string  RoleUniqueName { get; set; }

    #endregion
}
