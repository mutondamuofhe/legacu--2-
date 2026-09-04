using System.Security.Claims;
using LegacyVault.Data;
using LegacyVault.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LegacyVault.Controllers;
[Authorize] public class DocumentsController(ApplicationDbContext db, IWebHostEnvironment env) : Controller
{
    string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    public async Task<IActionResult> Index()=>View(await db.VaultDocuments.Where(x=>x.UserId==UserId).OrderByDescending(x=>x.CreatedAt).ToListAsync());
    public IActionResult Create()=>View();
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(IFormFile file, bool releaseAfterVerification){if(file is null||file.Length==0){ModelState.AddModelError("file","Choose a file.");return View();}var allowed=new[]{".pdf",".docx",".txt",".png",".jpg",".jpeg"};var ext=Path.GetExtension(file.FileName).ToLowerInvariant();if(!allowed.Contains(ext)||file.Length>10*1024*1024){ModelState.AddModelError("file","Use a PDF, DOCX, TXT, PNG, or JPG file up to 10 MB.");return View();}var folder=Path.Combine(env.ContentRootPath,"App_Data","uploads");Directory.CreateDirectory(folder);var stored=$"{Guid.NewGuid():N}{ext}";using var stream=System.IO.File.Create(Path.Combine(folder,stored));await file.CopyToAsync(stream);db.VaultDocuments.Add(new VaultDocument{UserId=UserId,DisplayName=Path.GetFileName(file.FileName),StoredFileName=stored,ContentType=file.ContentType,Size=file.Length,ReleaseAfterVerification=releaseAfterVerification});db.ActivityLogs.Add(new ActivityLog{UserId=UserId,Description=$"Uploaded document: {Path.GetFileName(file.FileName)}"});await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id){var item=await db.VaultDocuments.SingleOrDefaultAsync(x=>x.Id==id&&x.UserId==UserId);if(item is null)return NotFound();var path=Path.Combine(env.ContentRootPath,"App_Data","uploads",item.StoredFileName);if(System.IO.File.Exists(path))System.IO.File.Delete(path);db.Remove(item);await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
    public async Task<IActionResult> Download(int id){var item=await db.VaultDocuments.SingleOrDefaultAsync(x=>x.Id==id&&x.UserId==UserId);if(item is null)return NotFound();var path=Path.Combine(env.ContentRootPath,"App_Data","uploads",item.StoredFileName);if(!System.IO.File.Exists(path))return NotFound();return PhysicalFile(path,item.ContentType,item.DisplayName);}
}