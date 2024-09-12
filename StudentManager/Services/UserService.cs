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

        public bool InsertUser(long roleID, string userPhone, string userEmail, string userPassword, string createdBy)
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
                _userRepository.InsertUser(_user);
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

        public async Task<LoggedStudentDTO> GetLoggedStudentDetailAsync(long userId)
        {
            // Fetch users from the repository
            var StudentDetails = await _userRepository.GetLoggedStudentDetailAsync(userId);

            if (StudentDetails == null)
            {
                return null;
            }

            // Map the fetched data to the DTOs
            var loggedStudentDTO = new LoggedStudentDTO
            {
                UserId = StudentDetails.id_user,
                StudentPhone = StudentDetails.Student.user_phone,
                StudentEmail = StudentDetails.Student.user_email,
                StudentDetails = new List<StudentDetailDTO>
                {
                    new StudentDetailDTO
                    {
                        StudentDetailsId = StudentDetails.id_student_details,
                        StudentFirstName = StudentDetails.student_first_name,
                        StudentMiddleName = StudentDetails.student_middle_name,
                        StudentLastName = StudentDetails.student_last_name,
                        StudentAddress1 = StudentDetails.student_address1,
                        StudentAddress2 = StudentDetails.student_address2,
                        StudentCity = StudentDetails.student_city,
                        StudentDistrict = StudentDetails.student_district,
                        StudentState = StudentDetails.student_state,
                        StudentPIN = StudentDetails.student_pin,
                        StudentFeebookGiven = StudentDetails.student_feebook_given,
                        StudentCategory = new List<StudentCategoryDTO>
                        {
                            new StudentCategoryDTO
                            {
                                StudentCategoryId = StudentDetails.StudentCategory.id_student_category,
                                StudentCategoryName = StudentDetails.StudentCategory.student_category_name
                            }
                        }
                    }
                },
                StudentFees = StudentDetails.Student.StudentFees
                    .Select(fees => new StudentFeesDTO
                    {
                        StudentFeesId = fees.id_student_fees,
                        FeesForMonth = fees.fees_for_month,
                        FeesForYear = fees.fees_for_year,
                        FeeAmount = fees.fee_amount,
                        FeeBookEntryDone = fees.feebook_entry_done
                    }).ToList()
            };

            return loggedStudentDTO;
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
                new Claim("userID", user.id_user.ToString()),
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
