﻿using System.ComponentModel.DataAnnotations;
namespace TokenBased_Authentication.Domain.Entities.Role;

public sealed class Permission : BaseEntities<ulong>
{
    #region Properties

    public string Title { get; set; }

    public string PermissionUniqueName { get; set; }

    public ulong? ParentId { get; set; }

    #endregion
}
