using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendancePerformanceSystem.Data;
using StudentAttendancePerformanceSystem.Models;

namespace StudentAttendancePerformanceSystem.Controllers;

[Authorize(Roles = "Admin")]
public class TeachersController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var teachers = await db.Users
            .Where(u => u.Role == "Teacher")
            .OrderBy(u => u.FullName)
            .ToListAsync();
        return View(teachers);
    }

    public IActionResult Create() => View(new TeacherViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TeacherViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Password))
            ModelState.AddModelError("Password", "Password is required for a new teacher.");

        if (await db.Users.AnyAsync(u => u.Email.ToLower() == model.Email.Trim().ToLower()))
            ModelState.AddModelError("Email", "An account with this email address already exists.");

        if (!ModelState.IsValid) return View(model);

        var teacher = new AppUser
        {
            FullName = model.FullName.Trim(),
            Email = model.Email.Trim().ToLower(),
            Role = "Teacher",
            Department = model.Department?.Trim(),
            Subject = model.Subject?.Trim(),
            Phone = model.Phone?.Trim(),
            PasswordHash = PasswordService.Hash(model.Password!)
        };

        db.Users.Add(teacher);
        await db.SaveChangesAsync();
        TempData["Message"] = "Teacher added successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var teacher = await db.Users.FirstOrDefaultAsync(u => u.Id == id && u.Role == "Teacher");
        if (teacher == null) return NotFound();

        var model = new TeacherViewModel
        {
            Id = teacher.Id,
            FullName = teacher.FullName,
            Email = teacher.Email,
            Department = teacher.Department,
            Subject = teacher.Subject,
            Phone = teacher.Phone
        };
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TeacherViewModel model)
    {
        if (id != model.Id) return BadRequest();

        var teacher = await db.Users.FirstOrDefaultAsync(u => u.Id == id && u.Role == "Teacher");
        if (teacher == null) return NotFound();

        if (await db.Users.AnyAsync(u => u.Id != id && u.Email.ToLower() == model.Email.Trim().ToLower()))
            ModelState.AddModelError("Email", "Another user with this email address already exists.");

        if (!ModelState.IsValid) return View(model);

        teacher.FullName = model.FullName.Trim();
        teacher.Email = model.Email.Trim().ToLower();
        teacher.Department = model.Department?.Trim();
        teacher.Subject = model.Subject?.Trim();
        teacher.Phone = model.Phone?.Trim();

        if (!string.IsNullOrWhiteSpace(model.Password))
        {
            teacher.PasswordHash = PasswordService.Hash(model.Password);
        }

        await db.SaveChangesAsync();
        TempData["Message"] = "Teacher details updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var teacher = await db.Users.FirstOrDefaultAsync(u => u.Id == id && u.Role == "Teacher");
        if (teacher != null)
        {
            db.Users.Remove(teacher);
            await db.SaveChangesAsync();
            TempData["Message"] = "Teacher removed successfully.";
        }
        return RedirectToAction(nameof(Index));
    }
}
