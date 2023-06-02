using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;
using lab10.Models;

namespace lab10.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("Logged in") != null)
            return View("Index");
        else
            return View("Views/Auth/Login.cshtml");

    }

    [Route("logout/")]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("Logged in");
        return View("Views/Auth/Login.cshtml");
    }

    public IActionResult Privacy()
    {
        if (HttpContext.Session.GetString("Logged in") != null)
            return View();
        else
            return View("Views/Auth/Login.cshtml");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
