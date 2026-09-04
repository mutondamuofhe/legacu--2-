using System.Security.Claims;
using LegacyVault.Data;
using LegacyVault.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LegacyVault.Controllers;
[Authorize] public class ExecutorsController(ApplicationDbContext db) : Controller
{
    string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    public async Task<IActionResult> Index() => View(await db.DigitalExecutors.Where(x => x.UserId == UserId).ToListAsync());
    public IActionResult Create() => View(new DigitalExecutor());
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(DigitalExecutor executor) { if (!ModelState.IsValid) return View(executor); executor.UserId=UserId; db.Add(executor); db.ActivityLogs.Add(new ActivityLog { UserId=UserId, Description=$"Added executor: {executor.Name}" }); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> Edit(int id) => await db.DigitalExecutors.SingleOrDefaultAsync(x=>x.Id==id&&x.UserId==UserId) is { } item ? View(item) : NotFound();
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id, DigitalExecutor input) { var item=await db.DigitalExecutors.SingleOrDefaultAsync(x=>x.Id==id&&x.UserId==UserId);if(item is null)return NotFound();if(!ModelState.IsValid)return View(input);item.Name=input.Name;item.Email=input.Email;item.Phone=input.Phone;item.Relationship=input.Relationship;await db.SaveChangesAsync();return RedirectToAction(nameof(Index)); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id) { var item=await db.DigitalExecutors.SingleOrDefaultAsync(x=>x.Id==id&&x.UserId==UserId); if(item is null)return NotFound();db.Remove(item);await db.SaveChangesAsync();return RedirectToAction(nameof(Index)); }
}