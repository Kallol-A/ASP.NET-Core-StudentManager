# ASP.NET-Core-StudentManager

Building a Student Manager WebAPI project using ASP.NET Core 2.1 involves several steps. Here's a step-by-step guide to help you get started:

Step 1: Set Up Your Development Environment
Install .NET Core SDK:

Make sure you have .NET Core SDK 2.1 installed on your machine. You can download it from the .NET Core Download page.
Install Visual Studio:

Install Visual Studio 2017 or later, ensuring you include the ASP.NET and web development workload during installation.
Step 2: Create a New ASP.NET Core Web API Project
Open Visual Studio:

Launch Visual Studio.
Create a New Project:

Go to File > New > Project.
Select Project Template:

In the New Project dialog, select ASP.NET Core Web Application and click Next.
Configure Project:

Name your project (e.g., StudentManagerAPI), choose a location, and click Create.
Select API Template:

In the next dialog, choose API as the project template, make sure .NET Core and ASP.NET Core 2.1 are selected, and click Create.
Step 3: Set Up Your Project Structure
Project Structure:
Your project will have a basic structure with folders like Controllers, Models, and Properties.
Step 4: Create Models
Define Student Model:
Create a new folder named Models if it doesn't exist.
Add a new class file named Student.cs in the Models folder with the following content:
csharp
Copy code
namespace StudentManagerAPI.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Course { get; set; }
    }
}
Step 5: Set Up Your Database Context
Install Entity Framework Core:
Open the NuGet Package Manager Console and run the following commands to install Entity Framework Core and SQL Server provider:
sh
Copy code
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
Create Data Context:
Add a new class file named StudentContext.cs in the Models folder with the following content:
csharp
Copy code
using Microsoft.EntityFrameworkCore;

namespace StudentManagerAPI.Models
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}
Configure Database Connection:
Open appsettings.json and add your database connection string:
json
Copy code
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server_name;Database=StudentDB;Trusted_Connection=True;"
  },
  ...
}
Configure Services:
Open Startup.cs and add the following code in the ConfigureServices method:
csharp
Copy code
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<StudentContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
}
Step 6: Create Controllers
Create Student Controller:
Add a new folder named Controllers if it doesn't exist.
Add a new class file named StudentsController.cs in the Controllers folder with the following content:
csharp
Copy code
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagerAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentContext _context;

        public StudentsController(StudentContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
Step 7: Migrate Your Database
Add Initial Migration:
Open the NuGet Package Manager Console and run the following command:
sh
Copy code
Add-Migration InitialCreate
Update Database:
Run the following command to apply the migration and create the database:
sh
Copy code
Update-Database
Step 8: Run Your Application
Build and Run:

Press F5 or click the Run button to build and run your application.
Test Your API:

Use tools like Postman or curl to test your API endpoints (e.g., GET, POST, PUT, DELETE for /api/students).
Additional Steps
Add Validation: Add data annotations to your models for validation.
Authentication and Authorization: Implement authentication and authorization if required.
Documentation: Consider using Swagger for API documentation.
Unit Testing: Write unit tests for your controllers and services.
This guide provides a basic outline to get you started with building a Student Manager WebAPI using ASP.NET Core 2.1. You can expand upon this by adding more features and improving the architecture as needed.
