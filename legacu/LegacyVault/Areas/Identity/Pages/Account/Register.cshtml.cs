using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LegacyVault.Areas.Identity.Pages.Account;

public class RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string ReturnUrl { get; private set; } = "/Dashboard";

    public sealed class InputModel
    {
        [Required]
        [StringLength(120)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string IdNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public IActionResult OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/Dashboard");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/Dashboard");
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = new IdentityUser
        {
            UserName = Input.Email,
            Email = Input.Email,
            PhoneNumber = Input.PhoneNumber
        };
        var result = await userManager.CreateAsync(user, Input.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        await userManager.AddClaimsAsync(user,
        [
            new Claim("full_name", Input.FullName.Trim()),
            new Claim("id_number", Input.IdNumber.Trim())
        ]);
        await signInManager.SignOutAsync();
        TempData["RegistrationSuccess"] = "Your account has been created. Please log in to continue.";
        return RedirectToPage("./Login", new { returnUrl = ReturnUrl });
    }
}