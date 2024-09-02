using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using WebApi.DTOs;
using WebApi.Data;
using WebApi.Models;


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
                .Include(s => s.StudentCategory)
                .Include(s => s.Role)
                .Include(s => s.StudentFees)
                .FirstOrDefaultAsync(s => s.id_student == studentId);

            if (student == null || student.StudentDetails == null)
            {
                return null;
            }

            string fullAddress =
                (!string.IsNullOrEmpty(student.StudentDetails.student_address1) ? $"Locality: {student.StudentDetails.student_address1}" : "") +
                (!string.IsNullOrEmpty(student.StudentDetails.student_address2) ? $", {student.StudentDetails.student_address2}" : "") +
                (!string.IsNullOrEmpty(student.StudentDetails.student_city) ? $", City: {student.StudentDetails.student_city}" : "") +
                (!string.IsNullOrEmpty(student.StudentDetails.student_district) ? $", District: {student.StudentDetails.student_district}" : "") +
                (!string.IsNullOrEmpty(student.StudentDetails.student_pin) ? $", PIN: {student.StudentDetails.student_pin}" : "") +
                (!string.IsNullOrEmpty(student.StudentDetails.student_state) ? $", State: {student.StudentDetails.student_state}" : "");

            fullAddress = fullAddress.TrimStart(',').Trim();  // Remove any leading commas and trim the result

            List<StudentFeesDto> fees = student.StudentFees
                .AsEnumerable() // Convert ICollection to IEnumerable
                .Select(f => new StudentFeesDto
                {
                    FeesForMonth = f.fees_for_month,
                    FeesForYear = f.fees_for_year,
                    FeeAmount = f.fee_amount,
                    FeebookEntryDone = f.feebook_entry_done
                }).ToList();

            return new StudentDetailDto
            {
                FirstName = student.StudentDetails.student_first_name,
                MiddleName = student.StudentDetails.student_middle_name,
                LastName = student.StudentDetails.student_last_name,
                Address = fullAddress,
                Email = student.student_email,
                Category = student.StudentCategory.student_category,
                Role = student.Role.role,
                Fees = fees
            };
        }
    }
}
