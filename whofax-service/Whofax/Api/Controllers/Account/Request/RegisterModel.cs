using System.ComponentModel.DataAnnotations;
using Whofax.Api.Resources;

namespace Whofax.Controllers.Account.Request;

public class RegisterModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = nameof(DataAnnotationMessages.PasswordLengthRestriction), MinimumLength = 6)]
    public string Password { get; set; }

    [Compare(nameof(Password), ErrorMessage = nameof(DataAnnotationMessages.PasswordsDontMatch))]
    public string ConfirmPassword { get; set; }
}
