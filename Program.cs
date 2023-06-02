using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcPracownik.Data;
using Microsoft.Data.Sqlite;
using MvcPracownik.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MvcPracownikContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MvcPracownikContext") ?? throw new InvalidOperationException("Connection string 'MvcPracownikContext' not found.")));

var connectionStringBuilder = new SqliteConnectionStringBuilder();
connectionStringBuilder.DataSource = "loginy.db";

String sqlQueryForExistingUser = "SELECT login FROM loginy WHERE login='admin';";
using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
{
    connection.Open();
    using (var command = new SqliteCommand(sqlQueryForExistingUser, connection))
    {
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            if (!reader.Read())
            {
                String sql = "INSERT INTO loginy (login, haslo) VALUES ('admin', '21232f297a57a5a743894a0e4a801fc3');";
                using (var command2 = new SqliteCommand(sql, connection))
                {
                    command2.ExecuteNonQuery();
                }
            }
        }
    }
}

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;//plik cookie jest niedostępny przez skrypt po stronie klienta
    options.Cookie.IsEssential = true;//pliki cookie sesji będą zapisywane dzięki czemu sesje będzie mogła być śledzona podczas nawigacji lub przeładowania strony
});

var app = builder.Build();


//endpoint metody GET, przesyła listę wszystkich obiektów Informacja
app.MapGet("api/budynki", async (MvcPracownikContext db) =>
    await db.Budynek.ToListAsync());

//endpoint metody GET, pobiera obiekt Informacja o wybranym id
app.MapGet("api/budynki/{id}", async (int id, MvcPracownikContext db) =>
    await db.Budynek.FindAsync(id)
        is Budynek budynek
            ? Results.Ok(budynek)
            : Results.NotFound());

//endpoint metody POST, dodaje obiekt Informacja, pole klucza głównego (id) ma autoinkrement
app.MapPost("api/budynki", async (Budynek bud, MvcPracownikContext db) =>
{
    db.Budynek.Add(bud);
    await db.SaveChangesAsync();

    return Results.Created($"api/budynki/{bud.Id_budynku}", bud);
});

//endpoint metody PUT, modyfikuje obiekt o podanym id
app.MapPut("api/budynki/{id}", async (int id, Budynek inputInformacja, MvcPracownikContext db) =>
{
    var informacja = await db.Budynek.FindAsync(id);

    if (informacja is null) return Results.NotFound();

    informacja.Nazwa = inputInformacja.Nazwa;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

//endpoint metody DELETE, usuwa obiekt o podanym id
app.MapDelete("api/budynki/{id}", async (int id, MvcPracownikContext db) =>
{
    if (await db.Budynek.FindAsync(id) is Budynek informacja)
    {
        db.Budynek.Remove(informacja);
        await db.SaveChangesAsync();
        return Results.Ok(informacja);
    }

    return Results.NotFound();
});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Use(async (ctx, next) =>
{
    await next();

    if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
    {
        //Re-execute the request so the user gets the error page
        string originalPath = ctx.Request.Path.Value;
        ctx.Items["originalPath"] = originalPath;
        ctx.Request.Path = "/login";
        await next();
    }
});


app.Run();
