using System.ComponentModel.DataAnnotations;
namespace StudentAttendancePerformanceSystem.Models;
public class ExamMark
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student? Student { get; set; }
    [Required] public string Subject { get; set; } = "";
    [Required] public string ExamName { get; set; } = "";
    [Range(0, 100)] public decimal MarksObtained { get; set; }
    [Range(1, 100)] public decimal MaximumMarks { get; set; } = 100;
    public DateTime ExamDate { get; set; } = DateTime.Today;
}
