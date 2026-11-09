using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LegacyVault.Areas.Identity.Pages.Account;

public class LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string ReturnUrl { get; private set; } = "/Dashboard";

    public sealed class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }

    public IActionResult OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/Dashboard");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/Dashboard");
        await signInManager.SignOutAsync();
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            logger.LogInformation("User logged in.");
            return LocalRedirect(ReturnUrl);
        }

        ModelState.AddModelError(string.Empty, "Invalid email or password.");
        return Page();
    }
}