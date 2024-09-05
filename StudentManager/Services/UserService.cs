using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using StudentManager.Models;
using StudentManager.Repositories;
using StudentManager.DTOs;

namespace StudentManager.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasherService;

        // Constructor
        public UserService(IUserRepository userRepository, IPasswordHasherService passwordHasherService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasherService = passwordHasherService ?? throw new ArgumentNullException(nameof(passwordHasherService));
        }

        public bool AddUser(long roleID, string userPhone, string userEmail, string userPassword, string createdBy)
        {
            try
            {
                // Create a new User object
                var _user = new User
                {
                    id_role = roleID,
                    user_phone = userPhone,
                    user_email = userEmail,
                    user_password = userPassword,  // Password will be hashed in the repository
                    created_by_user = createdBy,
                    created_at = DateTime.Now
                };

                // Add user through repository
                _userRepository.AddUser(_user);
                _userRepository.Save();

                return true; // Operation succeeded
            }
            catch (Exception ex)
            {
                // Log the exception
                // e.g., _logger.LogError(ex, "Error occurred while adding a user.");

                return false; // Operation failed
            }
        }

        public async Task<LoginResult> LoginUser(string userEmail, string userPassword)
        {
            var user = await _userRepository.FindByEmailAsync(userEmail);

            if (user != null && _passwordHasherService.VerifyPassword(user.user_password, userPassword))
            {
                // Await the GenerateJwtToken method to get the token
                var token = await GenerateJwtToken(user.user_phone, userEmail);
                return new LoginResult { Success = true, Token = token, Message = "Login successful" };
            }

            return new LoginResult { Success = false, Token = null, Message = "Invalid email or password" };
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            // Fetch users from the repository
            var users = await _userRepository.GetUsersAsync();

            // Map the User entities to UserDTO objects
            var userDTOs = users.Select(u => new UserDTO
            {
                UserId = u.id_user,         // id_user is the primary key in the User entity
                RoleId = u.id_role,         // id_role is the foreign key related to Role entity
                UserPhone = u.user_phone,
                UserEmail = u.user_email,
                RoleName = u.Role.role_name // Navigation property Role and role_name in Role entity
            });

            return userDTOs;
        }

        public IEnumerable<UserDTO> GetUsersByRole(long roleId)
        {
            // Fetch users from the repository
            var users = _userRepository.GetUsersByRole(roleId);

            // Map the User entities to UserDTO objects
            var userDTOs = users.Select(u => new UserDTO
            {
                UserId = u.id_user,         // id_user is the primary key in the User entity
                RoleId = u.id_role,         // id_role is the foreign key related to Role entity
                UserPhone = u.user_phone,
                UserEmail = u.user_email,
                RoleName = u.Role.role_name // Navigation property Role and role_name in Role entity
            });

            return userDTOs;
        }

        public IEnumerable<UserDTO> GetUsersExceptRole(long roleId)
        {
            // Fetch users from the repository
            var users = _userRepository.GetUsersExceptRole(roleId);

            // Map the User entities to UserDTO objects
            var userDTOs = users.Select(u => new UserDTO
            {
                UserId = u.id_user,         // id_user is the primary key in the User entity
                RoleId = u.id_role,         // id_role is the foreign key related to Role entity
                UserPhone = u.user_phone,
                UserEmail = u.user_email,
                RoleName = u.Role.role_name // Navigation property Role and role_name in Role entity
            });

            return userDTOs;
        }

        private async Task<string> GenerateJwtToken(string userPhone, string userEmail)
        {
            // Fetch the user information, including the role, from the repository
            var user = await _userRepository.FindByEmailAsync(userEmail);

            if (user == null)
            {
                // Handle the case where the user is not found
                return null;
            }

            // Retrieve the user's role from the repository
            var role = await _userRepository.GetUserRole(user.id_role);

            if (role == null)
            {
                // Handle the case where the role is not found
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userEmail),
                new Claim(ClaimTypes.MobilePhone, userPhone),
                new Claim(ClaimTypes.Role, role.role_name),
                // Add more claims as needed
                new Claim("userID", role.id_role.ToString()),
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
