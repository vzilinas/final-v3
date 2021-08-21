using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Whofax.Controllers.Account.Request;
using Whofax.Domain.Entities.Identity;

namespace Whofax.Api.Controllers.Account;

/// <summary>
/// User Account Api
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AccountController : ApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <summary>
    /// Gets logged in user's information
    /// </summary>
    /// <returns>Currently logged in user's information</returns>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAppUser()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            return Ok(user);
        }
        await _signInManager.SignOutAsync();
        return Unauthorized();
    }

    /// <summary>
    /// Logs in the user defined in model
    /// </summary>
    /// <param name="model">User Login model</param>
    /// <returns>No Content</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return NoContent();
        }

        ModelState.AddModelError(string.Empty, "Username or password is not valid.");

        return BadRequest(ModelState);
    }

    /// <summary>
    /// Logs out currently logged in user
    /// </summary>
    /// <returns>No Content</returns>
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return NoContent();
    }

    /// <summary>
    /// Registers user
    /// </summary>
    /// <param name="model">User Register model</param>
    /// <returns>Ok</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        var user = new AppUser
        {
            UserName = model.Email,
            Email = model.Email
        };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return Ok();
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return BadRequest(ModelState);
    }

    /// <summary>
    /// Changes password for currently logged in user
    /// </summary>
    /// <param name="model">Change password model</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound("Logged in user not found.");
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        if (!changePasswordResult.Succeeded)
        {
            foreach (var error in changePasswordResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }

        await _signInManager.RefreshSignInAsync(user);
        return NoContent();
    }
}
