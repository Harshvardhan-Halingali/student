using System.ComponentModel.DataAnnotations;
namespace StudentAttendancePerformanceSystem.Models;
public class AttendanceRecord
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student? Student { get; set; }
    [DataType(DataType.Date)] public DateTime AttendanceDate { get; set; } = DateTime.Today;
    [Required] public string Status { get; set; } = "Present";
    public string? Remarks { get; set; }
}
