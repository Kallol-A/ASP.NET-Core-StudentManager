using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using WebApi.Data;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IUserService
    {
        bool AddUser(long roleID, string userEmail, string userPassword, string createdBy);

        Task<LoginResult> LoginUser(string userEmail, string userPassword);

        IEnumerable<User> GetAllUsers();
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IPasswordHasherService _passwordHasherService;

        // Constructor
        public UserService(AppDbContext dbContext, IPasswordHasherService passwordHasherService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _passwordHasherService = passwordHasherService ?? throw new ArgumentNullException(nameof(passwordHasherService));
        }

        public bool AddUser(long roleID, string userEmail, string userPassword, string createdBy)
        {
            try
            {
                var _user = new User
                {
                    id_role = roleID,
                    user_email = userEmail,
                    user_password = _passwordHasherService.HashPassword(userPassword),
                    created_by_user = createdBy,
                    created_at = DateTime.Now
                };

                _dbContext.Users.Add(_user);
                _dbContext.SaveChanges();

                return true; // Operation succeeded
            }
            catch (Exception ex)
            {
                // Log the exception

                return false; // Operation failed
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _dbContext.Users
                .Where(user => user.deleted_at == null)
                .ToList();
        }

        public async Task<LoginResult> LoginUser(string userEmail, string userPassword)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(User => User.user_email == userEmail && User.deleted_at == null);

            if (user != null && _passwordHasherService.VerifyPassword(user.user_password, userPassword))
            {
                var token = GenerateJwtToken(userEmail);
                return new LoginResult { Success = true, Token = token, Message = "Login successful" };
            }

            return new LoginResult { Success = false, Token = null, Message = "Invalid email or password" };
        }

        private string GenerateJwtToken(string userEmail)
        {
            // Fetch the user information, including the role, from the database
            var user = _dbContext.Users.FirstOrDefault(u => u.user_email == userEmail && u.deleted_at == null);

            if (user == null)
            {
                // Handle the case where the user is not found
                // You might want to log an error or throw an exception
                return null;
            }

            // Retrieve the user's role from the database
            var role = _dbContext.Roles.FirstOrDefault(r => r.id_role == user.id_role);

            if (role == null)
            {
                // Handle the case where the role is not found
                // You might want to log an error or throw an exception
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userEmail),
                new Claim(ClaimTypes.Role, role.role),
                // Add more claims as needed
                new Claim("roleID", role.id_role.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("jL0fcjRKi3YVNYBEo2VjnDGf4k1sFpX8v2P3VKwnTVY="));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "studentManager",
                audience: "stuman.com",
                claims: claims,
                expires: DateTime.Now.AddHours(24), // Token expiration time
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return accessToken;
        }
    }
}
