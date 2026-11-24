using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using StudentAttendancePerformanceSystem.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var isRender = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RENDER"));
var useSqlite = isRender || builder.Configuration["USE_SQLITE"] == "true" || (connectionString != null && connectionString.Contains(".db", StringComparison.OrdinalIgnoreCase));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (useSqlite)
    {
        var sqliteConn = connectionString?.Contains(".db") == true ? connectionString : "Data Source=campus.db";
        options.UseSqlite(sqliteConn);
    }
    else
    {
        options.UseSqlServer(connectionString ?? "Server=localhost;Database=StudentAttendancePerformanceDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
    }
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => { options.LoginPath = "/Account/Login"; options.AccessDeniedPath = "/Account/Denied"; });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();
    await DbSeeder.SeedAsync(db);
}

app.Run();
