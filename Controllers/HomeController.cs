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
            return View("HomePage");
        else
            return View();
    }

    [Route("Dodaj/")]
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

        return View("HomePage");
    }

    [Route("Wyswietl/")]
    public IActionResult Wyswietl()
    {

        if (HttpContext.Session.GetString("Logged in") == null)
            return View("Index");

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
                        Console.WriteLine(reader.GetInt32(0) + " " + reader.GetString(1));
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

    [Route("login/")]
    public IActionResult Login(String ulogin, String uhaslo)
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder();
        connectionStringBuilder.DataSource = "loginy.db";

        Encoding enc = Encoding.UTF8;
        var hashBuilder = new StringBuilder();
        using var hash = MD5.Create();
        byte[] result = hash.ComputeHash(enc.GetBytes(uhaslo));
        foreach (var b in result)
            hashBuilder.Append(b.ToString("x2"));
        String hasloHash = hashBuilder.ToString();

        Console.WriteLine(ulogin + " " + hasloHash);

        String sql = "SELECT login, haslo FROM loginy WHERE login='" + ulogin + "' AND haslo='" + hasloHash + "'";
        using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
        {
            connection.Open();
            using (var command = new SqliteCommand(sql, connection))
            {
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        HttpContext.Session.SetString("Logged in", ulogin);
                        return View("HomePage");
                    }
                    else
                    {
                        return View("Index");
                    }
                }
            }
        }

        // String databaseLogin = "admin", databaseHaslo = "admin";

        // if (ulogin == databaseLogin && uhaslo == databaseHaslo)
        // {
        //     HttpContext.Session.SetString("Logged in", ulogin);
        //     return View("HomePage");
        // }
        // else
        // {
        //     return View("Index");
        // }
    }

    [Route("logout/")]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("Logged in");
        return View("Index");
    }

    public IActionResult HomePage()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        if (HttpContext.Session.GetString("Logged in") != null)
            return View();
        else
            return View("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
