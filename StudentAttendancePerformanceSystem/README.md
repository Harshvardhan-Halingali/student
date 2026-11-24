# Student Attendance and Performance Management System

A Visual Studio 2022-compatible ASP.NET Core MVC project built with C#, .NET 8, Entity Framework Core, SQL Server LocalDB, Bootstrap and cookie authentication.

## Features
- Role-based login: Admin, Teacher and Student
- Dashboard with student, attendance and marks summaries
- Student record management (add, list, edit; admin can delete)
- Daily attendance recording, date filtering and duplicate prevention
- Exam marks entry with validation and percentage calculation
- Per-student attendance and marks report with print support
- Passwords stored as PBKDF2 hashes

## Requirements
- Visual Studio 2022 with the **ASP.NET and web development** workload
- .NET 8 SDK
- SQL Server LocalDB (usually installed with Visual Studio)

## Open and run
1. Extract the ZIP.
2. Open `StudentAttendancePerformanceSystem.sln` in Visual Studio 2022.
3. Allow NuGet packages to restore.
4. Check the connection string in `StudentAttendancePerformanceSystem/appsettings.json`. The default uses `(localdb)\MSSQLLocalDB`.
5. Set `StudentAttendancePerformanceSystem` as the startup project.
6. Press **F5** or **Ctrl+F5**. The app creates the database and sample records on first run.

## Demo logins
| Role | Email | Password |
|---|---|---|
| Admin | admin@campus.local | Admin@123 |
| Teacher | teacher@campus.local | Teacher@123 |
| Student | student@campus.local | Student@123 |

Change these demo passwords and configure production-grade secrets before deploying. The demo Student role is seeded for authentication; student self-service pages are not yet implemented. Teacher accounts are demo logins; account provisioning and subject/teacher assignment are not included in this starter version.

## Database
The database is named `StudentAttendancePerformanceDb`. It is created automatically using EF Core `EnsureCreated` at startup. For future schema changes, use EF Core migrations and replace `EnsureCreated` with `Migrate`.

## Notes
- Bootstrap is loaded from its public CDN, so an internet connection is needed for the default styling.
- This is a student-project starter, not a production-ready deployment. Add audit logging, stronger account administration, anti-forgery protection on all write actions (already enabled on forms), authorization policies for record ownership, and backups before real institutional use.
