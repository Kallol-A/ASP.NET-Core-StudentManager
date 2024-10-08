﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using StudentManager.Data;
using StudentManager.Models;
using StudentManager.Services;

namespace StudentManager.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private new readonly AppDbContext _dbContext;
        private readonly IPasswordHasherService _passwordHasherService;

        // Constructor
        public UserRepository(AppDbContext dbContext, IPasswordHasherService passwordHasherService) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _passwordHasherService = passwordHasherService ?? throw new ArgumentNullException(nameof(passwordHasherService));
        }

        public async void InsertUserAsync(User user)
        {
            var validUser = user ?? throw new ArgumentNullException(nameof(user));
            validUser.user_password = _passwordHasherService.HashPassword(validUser.user_password);
            await _dbContext.Users.AddAsync(validUser);
        }

        public async Task<User> FindByEmailAsync(string userEmail)
        {
            return await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.user_email == userEmail && u.deleted_at == null);
        }

        public async Task<Role> GetUserRole(long roleId)
        {
            return await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.id_role == roleId);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _dbContext.Users
                .Where(u => u.deleted_at == null)
                .Include(u => u.Role)  // Eagerly load the Role navigation property if needed
                .ToListAsync();
        }

        public Task<User> GetUserById(long userId)
        {
            return _dbContext.Users
                .Where(u => u.id_user == userId && u.deleted_at == null)
                .FirstOrDefaultAsync();
        }

        public IEnumerable<User> GetUsersByRole(List<long> roleIds)
        {
            return _dbContext.Users
                .Where(u => roleIds.Contains(u.id_role) && u.deleted_at == null)
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
    }
}
