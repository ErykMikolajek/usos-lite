using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;
using lab10.Models;

namespace lab10.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
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
                        return View("Views/Home/Index.cshtml");
                    }
                    else
                    {
                        TempData["msg"] = "<script>alert('Invalid username or password');</script>";
                        return RedirectToAction("LoginScreen");
                    }
                }
            }
        }
    }

    [Route("register/")]
    public IActionResult Register(String ulogin, String uhaslo)
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

        String sqlQueryForExistingUser = "SELECT login FROM loginy WHERE login='" + ulogin + "';";
        using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
        {
            connection.Open();
            using (var command = new SqliteCommand(sqlQueryForExistingUser, connection))
            {
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // komunikat o istniejącym
                        // return Content("<script language='javascript' type='text/javascript'>alert('Uzytkownik o podanej nazwie uzytkownika juz istnieje!');</script>");

                        TempData["msg"] = "<script>alert('Account with this username already exists!');</script>";

                    }
                    else
                    {
                        String sql = "INSERT INTO loginy (login, haslo) VALUES ('" + ulogin + "', '" + hasloHash + "');";
                        using (var command2 = new SqliteCommand(sql, connection))
                        {
                            command2.ExecuteNonQuery();
                        }
                        return View("Views/Home/Index.cshtml");
                    }
                }
            }
        }
        return View("Views/Auth/Register.cshtml");
    }

    public IActionResult LoginScreen()
    {
        return View("Views/Auth/Login.cshtml");
    }

    public IActionResult RegisterScreen()
    {
        return View("Views/Auth/Register.cshtml");
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
