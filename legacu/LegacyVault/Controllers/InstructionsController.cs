using System.Security.Claims;
using LegacyVault.Data;
using LegacyVault.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LegacyVault.Controllers;
[Authorize] public class InstructionsController(ApplicationDbContext db) : Controller
{
    string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    public async Task<IActionResult> Index() => View(await db.LegacyInstructions.Include(x=>x.DigitalAsset).Where(x=>x.UserId==UserId).ToListAsync());
    public async Task<IActionResult> Create() { ViewBag.Assets=await db.DigitalAssets.Where(x=>x.UserId==UserId).ToListAsync(); return View(new LegacyInstruction()); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(LegacyInstruction instruction) { if(!ModelState.IsValid){ViewBag.Assets=await db.DigitalAssets.Where(x=>x.UserId==UserId).ToListAsync();return View(instruction);} instruction.UserId=UserId; if(instruction.DigitalAssetId.HasValue&&!await db.DigitalAssets.AnyAsync(x=>x.Id==instruction.DigitalAssetId&&x.UserId==UserId)) return Forbid();db.Add(instruction);db.ActivityLogs.Add(new ActivityLog{UserId=UserId,Description=$"Added instruction: {instruction.Title}"});await db.SaveChangesAsync();return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> Edit(int id) { var item=await db.LegacyInstructions.SingleOrDefaultAsync(x=>x.Id==id&&x.UserId==UserId);if(item is null)return NotFound();ViewBag.Assets=await db.DigitalAssets.Where(x=>x.UserId==UserId).ToListAsync();return View(item); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id, LegacyInstruction input) { var item=await db.LegacyInstructions.SingleOrDefaultAsync(x=>x.Id==id&&x.UserId==UserId);if(item is null)return NotFound();if(!ModelState.IsValid){ViewBag.Assets=await db.DigitalAssets.Where(x=>x.UserId==UserId).ToListAsync();return View(input);}if(input.DigitalAssetId.HasValue&&!await db.DigitalAssets.AnyAsync(x=>x.Id==input.DigitalAssetId&&x.UserId==UserId))return Forbid();item.Title=input.Title;item.Details=input.Details;item.PreferredAction=input.PreferredAction;item.DigitalAssetId=input.DigitalAssetId;item.ReleaseAfterVerification=input.ReleaseAfterVerification;await db.SaveChangesAsync();return RedirectToAction(nameof(Index)); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id){var item=await db.LegacyInstructions.SingleOrDefaultAsync(x=>x.Id==id&&x.UserId==UserId);if(item is null)return NotFound();db.Remove(item);await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
}