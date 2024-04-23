using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TokenBased_Authentication.Domain.DTO.AdminSide.User;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.User.Command.EditUser;

public record EditUserAdminSideCommand : IRequest<EditUserResult>
{
    #region properties

    public ulong Id { get; set; }

    [DisplayName("Username")]
    [Required(ErrorMessage = "Please Enter {0}")]
    [MaxLength(200)]
    public string Username { get; set; }

    [MaxLength(200)]
    [DisplayName("Mobile")]
    [Required(ErrorMessage = "Please Enter {0}")]
    public string Mobile { get; set; }

    [DisplayName("Avatar")]
    public IFormFile? Avatar { get; set; }

    [Display(Name = "انتخاب نقش های کاربر")]
    public List<ulong>? UserRoles { get; set; }

    public bool IsActive { get; set; }

    #endregion
}
