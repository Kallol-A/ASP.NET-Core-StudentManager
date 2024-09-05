using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using StudentManager.Data;
using StudentManager.Models;
using StudentManager.Services;

namespace StudentManager.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IPasswordHasherService _passwordHasherService;

        public UserRepository(AppDbContext dbContext, IPasswordHasherService passwordHasherService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _passwordHasherService = passwordHasherService ?? throw new ArgumentNullException(nameof(passwordHasherService));
        }

        public void AddUser(User user)
        {
            var validUser = user ?? throw new ArgumentNullException(nameof(user));
            validUser.user_password = _passwordHasherService.HashPassword(validUser.user_password);
            _dbContext.Users
                .Add(validUser);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public async Task<User> FindByEmailAsync(string userEmail)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.user_email == userEmail && u.deleted_at == null);
        }

        public async Task<Role> GetUserRole(long id_role)
        {
            return await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.id_role == id_role);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _dbContext.Users
                .Where(u => u.deleted_at == null)
                .Include(u => u.Role)  // Eagerly load the Role navigation property if needed
                .ToListAsync();
        }

        public IEnumerable<User> GetUsersByRole(long roleId)
        {
            return _dbContext.Users
                .Where(u => u.id_role == roleId && u.deleted_at == null)
                .Include(u => u.Role)  // Eagerly load the Role navigation property if needed
                .ToList();
        }

        public IEnumerable<User> GetUsersExceptRole(long roleId)
        {
            return _dbContext.Users
                .Where(u => u.id_role != roleId && u.deleted_at == null)
                .Include(u => u.Role)  // Eagerly load the Role navigation property if needed
                .ToList();
        }

        public IEnumerable<User> GetLoggedInStudentDetails(long userId)
        {
            return _dbContext.Users
                .Where(u => u.id_user != userId && u.deleted_at == null)
                //.Include(u => u.de)  // Eagerly load the Role navigation property if needed
                .ToList();
        }
    }
}
