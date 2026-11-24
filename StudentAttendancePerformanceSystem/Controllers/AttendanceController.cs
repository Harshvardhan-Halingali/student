using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendancePerformanceSystem.Data;
using StudentAttendancePerformanceSystem.Models;
namespace StudentAttendancePerformanceSystem.Controllers;
[Authorize(Roles="Admin,Teacher")]
public class AttendanceController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(DateTime? date)
    {
        var day=(date??DateTime.Today).Date; ViewBag.Date=day.ToString("yyyy-MM-dd");
        return View(await db.AttendanceRecords.Include(x=>x.Student).Where(x=>x.AttendanceDate==day).OrderBy(x=>x.Student!.RollNumber).ToListAsync());
    }
    public async Task<IActionResult> Create()
    { ViewBag.Students=await db.Students.OrderBy(x=>x.FullName).ToListAsync(); return View(new AttendanceRecord()); }
    [HttpPost,ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AttendanceRecord model)
    {
        if(await db.AttendanceRecords.AnyAsync(x=>x.StudentId==model.StudentId && x.AttendanceDate==model.AttendanceDate.Date))
            ModelState.AddModelError("", "Attendance for this student and date already exists.");
        if(!ModelState.IsValid){ViewBag.Students=await db.Students.ToListAsync();return View(model);}
        model.AttendanceDate=model.AttendanceDate.Date; db.Add(model); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index),new {date=model.AttendanceDate.ToString("yyyy-MM-dd")});
    }
    [HttpPost,ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id){var x=await db.AttendanceRecords.FindAsync(id);if(x!=null){db.Remove(x);await db.SaveChangesAsync();}return RedirectToAction(nameof(Index));}
}
