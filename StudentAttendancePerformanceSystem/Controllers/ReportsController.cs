using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendancePerformanceSystem.Data;
namespace StudentAttendancePerformanceSystem.Controllers;
[Authorize]
public class ReportsController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var students=await db.Students.OrderBy(x=>x.RollNumber).ToListAsync();
        var rows=new List<ReportRow>();
        foreach(var s in students)
        {
            var records=await db.AttendanceRecords.Where(x=>x.StudentId==s.Id).ToListAsync();
            var marks=await db.ExamMarks.Where(x=>x.StudentId==s.Id).ToListAsync();
            rows.Add(new ReportRow(s.RollNumber,s.FullName,records.Count,records.Count(x=>x.Status=="Present"),
                marks.Count==0?0:(double)marks.Average(x=>x.MarksObtained/x.MaximumMarks*100)));
        }
        return View(rows);
    }
    public record ReportRow(string RollNumber,string FullName,int AttendanceDays,int PresentDays,double AveragePercentage);
}
