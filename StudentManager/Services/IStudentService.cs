using System.Collections.Generic;
using System.Threading.Tasks;

using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IStudentService
    {
        Task<LoginResult> LoginStudent(string studentEmail, string studentPassword);
        Task<StudentDetailDto> GetStudentDetailAsync(long studentId);

        bool AddStudent(long studentCategoryID, long roleID, string studentEmail, string studentPassword,
            string createdBy);
        bool AddStudentDetails(long studentID, string studentFName, string studentMName, string studentLName,
            string studentAddr1, string studentAddr2, string studentCity, string studentDistrict,
            string studentState, string studentPIN, bool studentFeebookGiven, string createdBy);
        IEnumerable<Student> GetAllStudents();
        IEnumerable<Student> GetAllStudentsWithAllDetails();
        IEnumerable<StudentDetails> GetAllStudentDetails();
    }
}