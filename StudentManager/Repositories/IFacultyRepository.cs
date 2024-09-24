using System.Collections.Generic;
using System.Threading.Tasks;

using StudentManager.DTOs;
using StudentManager.Models;

namespace StudentManager.Repositories
{
    public interface IFacultyRepository
    {
        void InsertFacultyDetailsAsync(FacultyDetails facultydetails);
        Task<bool> SaveAsync();
    }
}
