using System.ComponentModel.DataAnnotations;

namespace StudentAttendancePerformanceSystem.Models;

public class AppUser
{
    public int Id { get; set; }
    [Required, StringLength(100)] public string FullName { get; set; } = "";
    [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = "";
    [Required] public string PasswordHash { get; set; } = "";
    [Required] public string Role { get; set; } = "Student";
    [StringLength(100)] public string? Department { get; set; }
    [StringLength(100)] public string? Subject { get; set; }
    [StringLength(30)] public string? Phone { get; set; }
}
