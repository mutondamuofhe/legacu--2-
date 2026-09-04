using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LegacyVault.Models;

namespace LegacyVault.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View(new ContactMessageViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Contact(ContactMessageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        TempData["ContactSuccess"] = "Thanks for your message. We will be in touch soon.";
        return RedirectToAction(nameof(Contact));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
