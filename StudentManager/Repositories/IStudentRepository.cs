using System.Threading.Tasks;

using WebApi.DTOs;

namespace WebApi.Repositories
{
    public interface IStudentRepository
    {
        Task<StudentDetailDto> GetStudentDetailAsync(long studentId);
    }
}
