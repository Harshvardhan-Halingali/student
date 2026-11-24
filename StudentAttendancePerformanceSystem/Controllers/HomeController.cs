using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendancePerformanceSystem.Data;

namespace StudentAttendancePerformanceSystem.Controllers;

[Authorize]
public class HomeController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.StudentCount = await db.Students.CountAsync();
        ViewBag.TeacherCount = await db.Users.CountAsync(x => x.Role == "Teacher");
        ViewBag.AttendanceCount = await db.AttendanceRecords.CountAsync();
        ViewBag.MarkCount = await db.ExamMarks.CountAsync();
        ViewBag.Average = await db.ExamMarks.Select(x => (double?)(x.MarksObtained / x.MaximumMarks * 100)).AverageAsync() ?? 0;
        return View();
    }
}
