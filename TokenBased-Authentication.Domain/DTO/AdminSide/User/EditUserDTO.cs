using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace TokenBased_Authentication.Domain.DTO.AdminSide.User;

public record EditUserDTO
{
    #region properties

    public ulong Id { get; set; }

    [DisplayName("Username")]
    [Required(ErrorMessage = "Please Enter {0}")]
    [MaxLength(200)]
    public string? Username { get; set; }

    [MaxLength(200)]
    [DisplayName("Mobile")]
    [Required(ErrorMessage = "Please Enter {0}")]
    public string Mobile { get; set; }

    [Display(Name = "انتخاب نقش های کاربر")]
    public List<ulong>? UserRoles { get; set; }

    public IFormFile? UserAvatar { get; set; }

    public bool IsActive { get; set; }

    #endregion
}

public enum EditUserResult
{
    DuplicateEmail,
    DuplicateMobileNumber,
    Success,
    Error
}