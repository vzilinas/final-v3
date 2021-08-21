using System.ComponentModel.DataAnnotations;
using Whofax.Api.Resources;

namespace Whofax.Controllers.Account.Request;

public class ChangePasswordModel
{
    [Required]
    public string OldPassword { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = nameof(DataAnnotationMessages.PasswordLengthRestriction), MinimumLength = 6)]
    public string NewPassword { get; set; }

    [Compare(nameof(NewPassword), ErrorMessage = nameof(DataAnnotationMessages.ChangePasswordDontMatch))]
    public string ConfirmPassword { get; set; }
}
