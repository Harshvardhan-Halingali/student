using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendancePerformanceSystem.Data;
using StudentAttendancePerformanceSystem.Models;

namespace StudentAttendancePerformanceSystem.Controllers;

public class AccountController(AppDbContext db) : Controller
{
    [HttpGet]
    public IActionResult Login() => User.Identity?.IsAuthenticated == true ? RedirectToAction("Index", "Home") : View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "Please enter both email and password.";
            return View();
        }

        var normalizedEmail = email.Trim().ToLower();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Email.ToLower() == normalizedEmail);

        if (user == null || !PasswordService.Verify(password, user.PasswordHash))
        {
            ViewBag.Error = "Invalid email or password. Please verify your credentials or sign up for a new account.";
            return View();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register() => User.Identity?.IsAuthenticated == true ? RedirectToAction("Index", "Home") : View(new RegisterViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var normalizedEmail = model.Email.Trim().ToLower();
        if (await db.Users.AnyAsync(x => x.Email.ToLower() == normalizedEmail))
        {
            ModelState.AddModelError("Email", "An account with this email address already exists.");
            return View(model);
        }

        var user = new AppUser
        {
            FullName = model.FullName.Trim(),
            Email = normalizedEmail,
            PasswordHash = PasswordService.Hash(model.Password),
            Role = string.IsNullOrWhiteSpace(model.Role) ? "Student" : model.Role
        };

        db.Users.Add(user);

        // If the newly registered user is a Student, create a linked student record if not already present
        if (user.Role == "Student" && !await db.Students.AnyAsync(s => s.Email.ToLower() == normalizedEmail))
        {
            var studentCount = await db.Students.CountAsync();
            var roll = $"STU{(studentCount + 1):D3}";
            db.Students.Add(new Student
            {
                RollNumber = roll,
                FullName = user.FullName,
                Email = user.Email,
                Course = "General",
                Year = 1
            });
        }

        await db.SaveChangesAsync();

        // Sign in immediately upon registration
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

        TempData["Message"] = $"Welcome, {user.FullName}! Your {user.Role} account has been created.";
        return RedirectToAction("Index", "Home");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    public IActionResult Denied() => View();
}
