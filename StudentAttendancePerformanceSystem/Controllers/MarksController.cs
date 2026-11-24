using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendancePerformanceSystem.Data;
using StudentAttendancePerformanceSystem.Models;
namespace StudentAttendancePerformanceSystem.Controllers;
[Authorize(Roles="Admin,Teacher")]
public class MarksController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.ExamMarks.Include(x=>x.Student).OrderByDescending(x=>x.ExamDate).ToListAsync());
    public async Task<IActionResult> Create(){ViewBag.Students=await db.Students.OrderBy(x=>x.FullName).ToListAsync();return View(new ExamMark());}
    [HttpPost,ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ExamMark model)
    {
        if(model.MarksObtained>model.MaximumMarks) ModelState.AddModelError("MarksObtained","Marks cannot exceed maximum marks.");
        if(!ModelState.IsValid){ViewBag.Students=await db.Students.ToListAsync();return View(model);}
        db.Add(model);await db.SaveChangesAsync();return RedirectToAction(nameof(Index));
    }
    [HttpPost,ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id){var x=await db.ExamMarks.FindAsync(id);if(x!=null){db.Remove(x);await db.SaveChangesAsync();}return RedirectToAction(nameof(Index));}
}
