using System.Collections.Generic;
using System.Threading.Tasks;

using StudentManager.DTOs;
using StudentManager.Models;

namespace StudentManager.Services
{
    public interface IFacultyService
    {
        Task<bool> IsFaculty(FacultyDetails facultydetails);
        Task<bool> InsertFacultyDetailsAsync(FacultyDetails facultydetails);
    }
}
