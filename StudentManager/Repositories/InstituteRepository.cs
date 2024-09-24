using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using StudentManager.Data;
using StudentManager.Models;

namespace StudentManager.Repositories
{
    public class InstituteRepository : BaseRepository, IInstituteRepository
    {
        private new readonly AppDbContext _dbContext;

        // Constructor
        public InstituteRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async void InsertDepartmentAsync(Department department)
        {
            var validDepartment = department ?? throw new ArgumentNullException(nameof(department));
            await _dbContext.Departments.AddAsync(validDepartment);
        }
    }
}
