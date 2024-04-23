
using TokenBased_Authentication.Domain.DTO.Common;

namespace TokenBased_Authentication.Domain.DTO.AdminSide.User;

public class FilterUsersDTO : BasePaging<Entities.Account.User>
{
    #region properties

    public string? Username { get; set; }

    public string? Mobile { get; set; }

    #endregion
}
