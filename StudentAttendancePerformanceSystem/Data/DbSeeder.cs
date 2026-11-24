using Microsoft.EntityFrameworkCore;
using StudentAttendancePerformanceSystem.Models;
namespace StudentAttendancePerformanceSystem.Data;
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (!await db.Users.AnyAsync())
        {
            db.Users.AddRange(
                new AppUser { FullName="System Administrator", Email="admin@campus.local", Role="Admin", PasswordHash=PasswordService.Hash("Admin@123") },
                new AppUser { FullName="Demo Teacher", Email="teacher@campus.local", Role="Teacher", PasswordHash=PasswordService.Hash("Teacher@123") },
                new AppUser { FullName="Demo Student", Email="student@campus.local", Role="Student", PasswordHash=PasswordService.Hash("Student@123") });
        }
        if (!await db.Students.AnyAsync())
            db.Students.Add(new Student { RollNumber="STU001", FullName="Demo Student", Email="student@campus.local", Course="BCA", Year=2 });
        await db.SaveChangesAsync();
    }
}
