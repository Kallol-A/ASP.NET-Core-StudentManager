using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManager.Models
{
    public class User
    {
        [Key]
        public long id_user { get; set; }

        public string user_phone { get; set; }
        public string user_email { get; set; }
        public string user_password { get; set; }
        public string created_by_user { get; set; }
        public string last_updated_by_user { get; set; }
        public string deleted_by_user { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        [ForeignKey("Role")]
        public long id_role { get; set; }

        // Navigation property
        public virtual Role Role { get; set; }
        public virtual StudentDetails StudentDetails { get; set; } // One-to-One Relationship with StudentDetails
        public virtual ICollection<StudentFees> StudentFees { get; set; } // One-to-Many Relationship with StudentFees
    }

    public class LoginResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
