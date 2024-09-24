using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<bool> InsertUserAsync(User user)
        {
            try
            {
                // Create a new User object
                var _user = new User
                {
                    id_role = user.id_role,
                    user_phone = user.user_phone,
                    user_email = user.user_email,
                    user_password = user.user_password,  // Password will be hashed in the repository
                    created_by_user = user.created_by_user,
                    created_at = DateTime.Now
                };

                // Add user through repository
                _userRepository.InsertUserAsync(_user);
                var result = await _userRepository.SaveAsync();

                return result; // Operation succeeded
            }
            catch (Exception ex)
            {
                // Log the exception
                // e.g., _logger.LogError(ex, "Error occurred while adding a user.");

                return false; // Operation failed
            }
        }

        public async Task<LoginResult> LoginUser(User user)
        {
            var validuser = await _userRepository.FindByEmailAsync(user.user_email);

            if (validuser != null && _passwordHasherService.VerifyPassword(validuser.user_password, user.user_password))
            {
                // Await the GenerateJwtToken method to get the token
                var token = GenerateJwtToken(validuser);
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

        public IEnumerable<UserDTO> GetUsersByRole(List<long> roleIds)
        {
            // Fetch users from the repository
            var users = _userRepository.GetUsersByRole(roleIds);

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

        private string GenerateJwtToken(User user)
        {
            if (user == null)
            {
                // Handle the case where the user is not found
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.user_email),
                new Claim(ClaimTypes.MobilePhone, user.user_phone),
                new Claim(ClaimTypes.Role, user.Role.role_name),
                // Add more claims as needed
                new Claim("userID", user.id_user.ToString()),
                new Claim("roleID", user.id_role.ToString()),
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
