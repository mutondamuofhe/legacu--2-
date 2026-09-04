using System.Security.Claims;
using LegacyVault.Data;
using LegacyVault.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LegacyVault.Controllers;

[Authorize]
public class DashboardController(ApplicationDbContext db, UserManager<IdentityUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userManager.GetUserAsync(User);
        var counts = await Task.WhenAll(
            db.DigitalAssets.CountAsync(x => x.UserId == userId),
            db.VaultDocuments.CountAsync(x => x.UserId == userId),
            db.DigitalExecutors.CountAsync(x => x.UserId == userId),
            db.LegacyInstructions.CountAsync(x => x.UserId == userId));
        var completed = counts.Count(x => x > 0);
        return View(new DashboardViewModel
        {
            UserName = user?.UserName?.Split('@')[0] ?? "there",
            AssetCount = counts[0], DocumentCount = counts[1],
            ExecutorCount = counts[2], InstructionCount = counts[3],
            HealthPercent = completed * 25,
            RecentActivity = await db.ActivityLogs.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt).Take(5).ToListAsync()
        });
    }
}