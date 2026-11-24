using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendancePerformanceSystem.Data;
using StudentAttendancePerformanceSystem.Models;
namespace StudentAttendancePerformanceSystem.Controllers;
[Authorize(Roles="Admin,Teacher")]
public class StudentsController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Students.OrderBy(x=>x.RollNumber).ToListAsync());
    public IActionResult Create() => View(new Student());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Student model)
    {
        if (await db.Students.AnyAsync(x=>x.RollNumber==model.RollNumber)) ModelState.AddModelError("RollNumber","Roll number already exists.");
        if (!ModelState.IsValid) return View(model);
        db.Students.Add(model); await db.SaveChangesAsync(); TempData["Message"]="Student added."; return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id) { var item=await db.Students.FindAsync(id); return item==null?NotFound():View(item); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Student model)
    {
        if(id!=model.Id) return BadRequest();
        if(!ModelState.IsValid) return View(model);
        db.Update(model); await db.SaveChangesAsync(); TempData["Message"]="Student updated."; return RedirectToAction(nameof(Index));
    }
    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles="Admin")]
    public async Task<IActionResult> Delete(int id) { var s=await db.Students.FindAsync(id); if(s!=null){db.Students.Remove(s);await db.SaveChangesAsync();} return RedirectToAction(nameof(Index)); }
}
