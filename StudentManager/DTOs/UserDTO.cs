using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.DTOs
{
    public class CreateUserDTO
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
    }

    public class UserDTO
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string RoleName { get; set; }
    }

    public class LoggedStudentDTO
    {
        public long UserId { get; set; }
        public string StudentPhone { get; set; }
        public string StudentEmail { get; set; }
        public List<StudentDetailsDTO> StudentDetails { get; set; }
    }

    public class StudentDetailsDTO
    {
        public long StudentDetailsId { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentMiddleName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentAddress1 { get; set; }
        public string StudentAddress2 { get; set; }
        public string StudentCity { get; set; }
        public string StudentDistrict { get; set; }
        public string StudentState { get; set; }
        public string StudentPIN { get; set; }
        public bool StudentFeebookGiven { get; set; }
        public List<StudentCategoryDTO> StudentCategory { get; set; }
    }

    public class StudentCategoryDTO
    {
        public long StudentCategoryId { get; set; }
        public string StudentCategoryName { get; set; }
    }
}
