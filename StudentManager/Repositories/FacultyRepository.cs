using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using StudentManager.Data;
using StudentManager.Models;

namespace StudentManager.Repositories
{
    public class FacultyRepository : BaseRepository, IFacultyRepository
    {
        private new readonly AppDbContext _dbContext;

        // Constructor
        public FacultyRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async void InsertFacultyDetailsAsync(FacultyDetails facultydetails)
        {
            var validFacultyDetails = facultydetails ?? throw new ArgumentNullException(nameof(facultydetails));
            await _dbContext.FacultyDetails.AddAsync(validFacultyDetails);
        }
    }
}
