using LegacyVault.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LegacyVault.Controllers;
[Authorize(Roles = "Admin")] public class AdminController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.VerificationRequests.OrderByDescending(x => x.RequestedAt).ToListAsync());
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Review(int id, string status, string? notes) { var request=await db.VerificationRequests.FindAsync(id);if(request is null)return NotFound();request.Status=status is "Approved" or "Rejected"?status:"Pending";request.Notes=notes;request.ReviewedAt=DateTime.UtcNow;await db.SaveChangesAsync();return RedirectToAction(nameof(Index)); }
}