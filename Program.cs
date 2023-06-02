using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcPracownik.Data;
using Microsoft.Data.Sqlite;
using MvcPracownik.Models;
using Microsoft.AspNetCore.Builder;

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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

//BUDYNKI:

//endpoint metody GET, przesyła listę wszystkich obiektów Informacja
app.MapGet("api/{token}/budynki", async (String token, MvcPracownikContext db) =>
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

//STUDENCI:

//endpoint metody GET, przesyła listę wszystkich obiektów Informacja
app.MapGet("api/studenci", async (MvcPracownikContext db) =>
    await db.Student.ToListAsync());
//endpoint metody GET, pobiera obiekt Informacja o wybranym id
app.MapGet("api/studenci/{id}", async (int id, MvcPracownikContext db) =>
    await db.Student.FindAsync(id)
        is Student student
            ? Results.Ok(student)
            : Results.NotFound());

//endpoint metody POST, dodaje obiekt Informacja, pole klucza głównego (id) ma autoinkrement
app.MapPost("api/studenci", async (Student stud, MvcPracownikContext db) =>
{
    db.Student.Add(stud);
    await db.SaveChangesAsync();

    return Results.Created($"api/studenci/{stud.Id}", stud);
});

//endpoint metody PUT, modyfikuje obiekt o podanym id
app.MapPut("api/studenci/{id}", async (int id, Student inputInformacja, MvcPracownikContext db) =>
{
    var informacja = await db.Student.FindAsync(id);

    if (informacja is null) return Results.NotFound();

    informacja.Imie = inputInformacja.Imie;
    informacja.Nazwisko = inputInformacja.Nazwisko;
    informacja.DataOfStudiesStart = inputInformacja.DataOfStudiesStart;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

//endpoint metody DELETE, usuwa obiekt o podanym id
app.MapDelete("api/studenci/{id}", async (int id, MvcPracownikContext db) =>
{
    if (await db.Student.FindAsync(id) is Student informacja)
    {
        db.Student.Remove(informacja);
        await db.SaveChangesAsync();
        return Results.Ok(informacja);
    }

    return Results.NotFound();
});

//PRACOWNICY:

//endpoint metody GET, przesyła listę wszystkich obiektów Informacja
app.MapGet("api/pracownicy", async (MvcPracownikContext db) =>
    await db.Pracownik.ToListAsync());
//endpoint metody GET, pobiera obiekt Informacja o wybranym id
app.MapGet("api/pracownicy/{id}", async (int id, MvcPracownikContext db) =>
    await db.Pracownik.FindAsync(id)
        is Pracownik pracownik
            ? Results.Ok(pracownik)
            : Results.NotFound());

//endpoint metody POST, dodaje obiekt Informacja, pole klucza głównego (id) ma autoinkrement
app.MapPost("api/pracownicy", async (Pracownik stud, MvcPracownikContext db) =>
{
    db.Pracownik.Add(stud);
    await db.SaveChangesAsync();

    return Results.Created($"api/pracownicy/{stud.Id}", stud);
});

//endpoint metody PUT, modyfikuje obiekt o podanym id
app.MapPut("api/pracownicy/{id}", async (int id, Pracownik inputInformacja, MvcPracownikContext db) =>
{
    var informacja = await db.Pracownik.FindAsync(id);

    if (informacja is null) return Results.NotFound();

    informacja.Imie = inputInformacja.Imie;
    informacja.Nazwisko = inputInformacja.Nazwisko;
    informacja.DataZatrudnienia = inputInformacja.DataZatrudnienia;
    informacja.Zajecia = inputInformacja.Zajecia;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

//endpoint metody DELETE, usuwa obiekt o podanym id
app.MapDelete("api/pracownicy/{id}", async (int id, MvcPracownikContext db) =>
{
    if (await db.Pracownik.FindAsync(id) is Pracownik informacja)
    {
        db.Pracownik.Remove(informacja);
        await db.SaveChangesAsync();
        return Results.Ok(informacja);
    }

    return Results.NotFound();
});

//ZAJECIA:

//endpoint metody GET, przesyła listę wszystkich obiektów Informacja
app.MapGet("api/zajecia", async (MvcPracownikContext db) =>
    await db.Zajecia.ToListAsync());
//endpoint metody GET, pobiera obiekt Informacja o wybranym id
app.MapGet("api/zajecia/{id}", async (int id, MvcPracownikContext db) =>
    await db.Zajecia.FindAsync(id)
        is Zajecia zaj
            ? Results.Ok(zaj)
            : Results.NotFound());

//endpoint metody POST, dodaje obiekt Informacja, pole klucza głównego (id) ma autoinkrement
app.MapPost("api/zajecia", async (Zajecia zaj, MvcPracownikContext db) =>
{
    db.Zajecia.Add(zaj);
    await db.SaveChangesAsync();

    return Results.Created($"api/zajecia/{zaj.Id_zajec}", zaj);
});

//endpoint metody PUT, modyfikuje obiekt o podanym id
app.MapPut("api/zajecia/{id}", async (int id, Zajecia inputInformacja, MvcPracownikContext db) =>
{
    var informacja = await db.Zajecia.FindAsync(id);

    if (informacja is null) return Results.NotFound();

    informacja.Nazwa = inputInformacja.Nazwa;
    informacja.Budynek = inputInformacja.Budynek;
    informacja.student_Zajecia = inputInformacja.student_Zajecia;
    informacja.Pracownicy = inputInformacja.Pracownicy;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

//endpoint metody DELETE, usuwa obiekt o podanym id
app.MapDelete("api/zajecia/{id}", async (int id, MvcPracownikContext db) =>
{
    if (await db.Zajecia.FindAsync(id) is Zajecia informacja)
    {
        db.Zajecia.Remove(informacja);
        await db.SaveChangesAsync();
        return Results.Ok(informacja);
    }

    return Results.NotFound();
});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // app.UseSwagger();
    // app.UseSwaggerUI();
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
