using Hw7.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hw7.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult UserProfile()
    {
        return View(new UserProfile());
    }

    [HttpPost]
    public IActionResult UserProfile(UserProfile profile)
    {
        return View(profile);
    }
}