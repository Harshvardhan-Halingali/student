using System.ComponentModel.DataAnnotations;
namespace StudentAttendancePerformanceSystem.Models;
public class Student
{
    public int Id { get; set; }
    [Required, StringLength(30)] public string RollNumber { get; set; } = "";
    [Required, StringLength(100)] public string FullName { get; set; } = "";
    [Required, EmailAddress] public string Email { get; set; } = "";
    [StringLength(80)] public string Course { get; set; } = "";
    public int Year { get; set; } = 1;
}
