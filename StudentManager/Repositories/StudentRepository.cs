using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using WebApi.DTOs;
using WebApi.Data;

namespace WebApi.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StudentDetailDto> GetStudentDetailAsync(long studentId)
        {
            var student = await _context.Students
                .Include(s => s.StudentDetails)
                .FirstOrDefaultAsync(s => s.id_student == studentId);

            if (student == null || student.StudentDetails == null)
            {
                return null;
            }

            return new StudentDetailDto
            {
                FirstName = student.StudentDetails.student_first_name,
                MiddleName = student.StudentDetails.student_middle_name,
                LastName = student.StudentDetails.student_last_name,
                Email = student.student_email
            };
        }
    }
}
