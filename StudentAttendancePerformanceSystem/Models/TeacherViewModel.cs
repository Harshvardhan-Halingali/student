using System.ComponentModel.DataAnnotations;

namespace StudentAttendancePerformanceSystem.Models;

public class TeacherViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Full Name is required."), StringLength(100)]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = "";

    [Required(ErrorMessage = "Email is required."), EmailAddress, StringLength(150)]
    public string Email { get; set; } = "";

    [StringLength(100)]
    [Display(Name = "Department")]
    public string? Department { get; set; }

    [StringLength(100)]
    [Display(Name = "Subject / Specialization")]
    public string? Subject { get; set; }

    [StringLength(30)]
    [Phone]
    [Display(Name = "Phone Number")]
    public string? Phone { get; set; }

    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string? Password { get; set; }
}
