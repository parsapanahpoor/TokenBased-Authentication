namespace TokenBased_Authentication.Domain.DTO.AdminSide.Role;

public record CreateRoleDTO
{
    #region Role Title

    public string Title { get; set; }

    public string RoleUniqueName { get; set; }

    public List<ulong>? Permissions { get; set; }

    #endregion
}
