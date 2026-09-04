using System.Security.Claims;
using LegacyVault.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LegacyVault.Controllers;
[Authorize] public class ActivityController(ApplicationDbContext db) : Controller
{ public async Task<IActionResult> Index(){var userId=User.FindFirstValue(ClaimTypes.NameIdentifier)!;return View(await db.ActivityLogs.Where(x=>x.UserId==userId).OrderByDescending(x=>x.CreatedAt).ToListAsync());} }