using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LegacyVault.Areas.Identity.Pages.Account.Manage;

public class IndexModel(UserManager<IdentityUser> userManager) : PageModel
{
    public string FullName { get; private set; } = "Legacy Vault Member";
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string IdNumber { get; private set; } = string.Empty;
    public string Initials { get; private set; } = "LV";

    public async Task OnGetAsync()
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null) return;

        Email = user.Email ?? string.Empty;
        PhoneNumber = user.PhoneNumber ?? string.Empty;
        var claims = await userManager.GetClaimsAsync(user);
        var fullName = claims.FirstOrDefault(x => x.Type == "full_name")?.Value;
        FullName = !string.IsNullOrWhiteSpace(fullName)
            ? fullName
            : User.FindFirstValue(ClaimTypes.Name) ?? Email;

        IdNumber = claims.FirstOrDefault(x => x.Type == "id_number")?.Value ?? string.Empty;
        Initials = string.Join(string.Empty, FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2).Select(x => char.ToUpperInvariant(x[0])));
        if (string.IsNullOrEmpty(Initials)) Initials = "LV";
    }
}