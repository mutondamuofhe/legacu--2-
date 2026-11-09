using System.Security.Claims;
using LegacyVault.Data;
using LegacyVault.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LegacyVault.Controllers;

[Authorize]
public class AssetsController(ApplicationDbContext db, IDataProtectionProvider protectionProvider) : Controller
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    private static readonly string[] Categories = ["Banking", "Social", "Email", "Entertainment", "Shopping", "Cloud Storage", "Utilities", "Other"];
    private readonly IDataProtector credentialsProtector = protectionProvider.CreateProtector("LegacyVault.DigitalAsset.Credentials.v1");

    public async Task<IActionResult> Index(string? category = null)
    {
        var assets = db.DigitalAssets.Where(x => x.UserId == UserId);
        if (!string.IsNullOrWhiteSpace(category) && Categories.Contains(category, StringComparer.OrdinalIgnoreCase))
        {
            assets = assets.Where(x => x.Category == category);
        }

        ViewBag.Categories = Categories;
        ViewBag.SelectedCategory = category;
        return View(await assets.OrderByDescending(x => x.CreatedAt).ToListAsync());
    }

    public IActionResult Create()
    {
        ViewBag.Categories = Categories;
        return View(new DigitalAsset());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DigitalAsset asset)
    {
        ModelState.Remove(nameof(DigitalAsset.UserId));
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = Categories;
            return View(asset);
        }

        asset.UserId = UserId;
        asset.EncryptedCredentials = ProtectCredentials(asset.Credentials);
        asset.Credentials = string.Empty;
        db.DigitalAssets.Add(asset);
        db.ActivityLogs.Add(new ActivityLog { UserId = UserId, Description = $"Added digital asset: {asset.Name}" });
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var asset = await db.DigitalAssets.SingleOrDefaultAsync(x => x.Id == id && x.UserId == UserId);
        if (asset is null) return NotFound();

        ViewBag.Categories = Categories;
        return View(asset);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DigitalAsset input)
    {
        var asset = await db.DigitalAssets.SingleOrDefaultAsync(x => x.Id == id && x.UserId == UserId);
        if (asset is null) return NotFound();
        ModelState.Remove(nameof(DigitalAsset.UserId));
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = Categories;
            return View(input);
        }

        asset.Name = input.Name;
        asset.Category = input.Category;
        asset.Provider = input.Provider;
        asset.AccountReference = input.AccountReference;
        asset.PreferredAction = input.PreferredAction;
        asset.ReleaseAfterVerification = input.ReleaseAfterVerification;
        if (!string.IsNullOrWhiteSpace(input.Credentials))
        {
            asset.EncryptedCredentials = ProtectCredentials(input.Credentials);
        }

        db.ActivityLogs.Add(new ActivityLog { UserId = UserId, Description = $"Updated digital asset: {asset.Name}" });
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var asset = await db.DigitalAssets.SingleOrDefaultAsync(x => x.Id == id && x.UserId == UserId);
        if (asset is null) return NotFound();

        db.ActivityLogs.Add(new ActivityLog { UserId = UserId, Description = $"Deleted digital asset: {asset.Name}" });
        db.DigitalAssets.Remove(asset);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private string? ProtectCredentials(string? credentials) =>
        string.IsNullOrWhiteSpace(credentials) ? null : credentialsProtector.Protect(credentials);

}