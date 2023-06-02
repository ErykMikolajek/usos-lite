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

    [Route("dodaj/")]
    public IActionResult Dodaj(String wpis_)
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder();
        connectionStringBuilder.DataSource = "dane.db";

        String sql = "INSERT INTO dane (wpis) VALUES ('" + wpis_ + "');";
        using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
        {
            connection.Open();
            using (var command = new SqliteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        return View("Index");
    }

    [Route("Wyswietl/")]
    public IActionResult Wyswietl()
    {

        if (HttpContext.Session.GetString("Logged in") == null)
            return View("Views/Auth/Login.cshtml");

        var connectionStringBuilder = new SqliteConnectionStringBuilder();
        connectionStringBuilder.DataSource = "dane.db";
        String sql = "SELECT * FROM dane;";

        List<int> numery = new List<int>();
        List<String> wiadomosci = new List<String>();

        using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
        {
            connection.Open();
            using (var command = new SqliteCommand(sql, connection))
            {
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        numery.Add(reader.GetInt32(0));
                        wiadomosci.Add(reader.GetString(1));
                    }
                }
            }
        }

        ViewData["numer_wpisu"] = numery;
        ViewData["wpis"] = wiadomosci;

        return View();
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
