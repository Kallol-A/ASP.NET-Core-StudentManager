using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

using WebApi.Data;
using WebApi.DTOs;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _dbContext;
        private readonly IStudentRepository _studentRepository;
        private readonly IPasswordHasherService _passwordHasherService;

        // Constructor
        public StudentService(AppDbContext dbContext, IPasswordHasherService passwordHasherService, IStudentRepository studentRepository)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _passwordHasherService = passwordHasherService ?? throw new ArgumentNullException(nameof(passwordHasherService));
            _studentRepository = studentRepository;
        }

        public bool AddStudent(long studentCategoryID, long roleID, string studentEmail,
            string studentPassword, string createdBy)
        {
            try
            {
                var _student = new Student
                {
                    id_student_category = studentCategoryID,
                    id_role = roleID,
                    student_email = studentEmail,
                    student_password = _passwordHasherService.HashPassword(studentPassword),
                    created_by_user = createdBy,
                    created_at = DateTime.Now
                };

                _dbContext.Students.Add(_student);
                _dbContext.SaveChanges();

                return true; // Operation succeeded
            }
            catch (Exception ex)
            {
                // Log the exception

                return false; // Operation failed
            }
        }

        public bool AddStudentDetails(long studentID, string studentFName, string studentMName,
            string studentLName, string studentAddr1, string studentAddr2, string studentCity,
            string studentDistrict, string studentState, string studentPIN, bool studentFeebookGiven,
            string createdBy)
        {
            try
            {
                var _studentdetails = new StudentDetails
                {
                    id_student = studentID,
                    student_first_name = studentFName,
                    student_middle_name = studentMName,
                    student_last_name = studentLName,
                    student_address1 = studentAddr1,
                    student_address2 = studentAddr2,
                    student_city = studentCity,
                    student_district = studentDistrict,
                    student_state = studentState,
                    student_pin = studentPIN,
                    student_feebook_given = studentFeebookGiven,
                    created_by_user = createdBy,
                    created_at = DateTime.Now
                };

                _dbContext.StudentDetails.Add(_studentdetails);
                _dbContext.SaveChanges();

                return true; // Operation succeeded
            }
            catch (Exception ex)
            {
                // Log the exception

                return false; // Operation failed
            }
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _dbContext.Students
                .Include(student => student.StudentDetails)
                .Where(student => student.deleted_at == null)
                .ToList();
        }

        public IEnumerable<StudentDetails> GetAllStudentDetails()
        {
            return _dbContext.StudentDetails
                .Where(student_detail => student_detail.deleted_at == null)
                .ToList();
        }

        public IEnumerable<Student> GetAllStudentsWithAllDetails()
        {
            return _dbContext.Students
                .Include(student => student.StudentDetails) // Include StudentDetails
                //.Include(student => student.StudentFees)    // Include StudentFees
                .Where(student => student.deleted_at == null)
                .ToList();
        }

        public Task<StudentDetailDto> GetStudentDetailAsync(long studentId)
        {
            return _studentRepository.GetStudentDetailAsync(studentId);
        }

        public async Task<LoginResult> LoginStudent(string studentEmail, string studentPassword)
        {
            var student = await _dbContext.Students
                .FirstOrDefaultAsync(Student => Student.student_email == studentEmail && Student.deleted_at == null);

            if (student != null && _passwordHasherService.VerifyPassword(student.student_password, studentPassword))
            {
                var token = GenerateJwtToken(studentEmail);
                return new LoginResult { Success = true, Token = token, Message = "Login successful" };
            }

            return new LoginResult { Success = false, Token = null, Message = "Invalid email or password" };
        }

        private string GenerateJwtToken(string studentEmail)
        {
            // Fetch the user information, including the role, from the database
            var student = _dbContext.Students.FirstOrDefault(s => s.student_email == studentEmail && s.deleted_at == null);

            if (student == null)
            {
                // Handle the case where the user is not found
                // You might want to log an error or throw an exception
                return null;
            }

            // Retrieve the user's role from the database
            var role = _dbContext.Roles.FirstOrDefault(r => r.id_role == student.id_role);

            if (role == null)
            {
                // Handle the case where the role is not found
                // You might want to log an error or throw an exception
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, studentEmail),
                new Claim(ClaimTypes.Role, role.role),
                // Add more claims as needed
                new Claim("studentID", student.id_student.ToString()),
                new Claim("roleID", role.id_role.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("jL0fcjRKi3YVNYBEo2VjnDGf4k1sFpX8v2P3VKwnTVY="));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "studentManager",
                audience: "stuman.com",
                claims: claims,
                expires: DateTime.Now.AddHours(24), // Token expiration time
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return accessToken;
        }
    }

    public class LoginResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
