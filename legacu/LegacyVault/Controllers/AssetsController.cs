using System.Security.Claims;
using LegacyVault.Data;
using LegacyVault.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LegacyVault.Controllers;

[Authorize]
public class AssetsController(ApplicationDbContext db) : Controller
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    public async Task<IActionResult> Index() => View(await db.DigitalAssets.Where(x => x.UserId == UserId).OrderByDescending(x => x.CreatedAt).ToListAsync());
    public IActionResult Create() => View(new DigitalAsset());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DigitalAsset asset)
    {
        if (!ModelState.IsValid) return View(asset);
        asset.UserId = UserId; db.Add(asset); db.ActivityLogs.Add(new ActivityLog { UserId = UserId, Description = $"Added digital asset: {asset.Name}" }); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id) => await db.DigitalAssets.SingleOrDefaultAsync(x => x.Id == id && x.UserId == UserId) is { } asset ? View(asset) : NotFound();
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DigitalAsset input)
    {
        var asset = await db.DigitalAssets.SingleOrDefaultAsync(x => x.Id == id && x.UserId == UserId); if (asset is null) return NotFound();
        if (!ModelState.IsValid) return View(input); asset.Name = input.Name; asset.Category = input.Category; asset.Provider = input.Provider; asset.AccountReference = input.AccountReference; asset.PreferredAction = input.PreferredAction; asset.ReleaseAfterVerification = input.ReleaseAfterVerification; await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id) { var asset = await db.DigitalAssets.SingleOrDefaultAsync(x => x.Id == id && x.UserId == UserId); if (asset is null) return NotFound(); db.Remove(asset); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
}