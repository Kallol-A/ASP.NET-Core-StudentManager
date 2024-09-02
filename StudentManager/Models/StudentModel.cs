using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Student
    {
        [Key]
        public long id_student { get; set; }

        public string student_email { get; set; }
        public string student_password { get; set; }
        public string created_by_user { get; set; }
        public string last_updated_by_user { get; set; }
        public string deleted_by_user { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        [ForeignKey("StudentCategory")]
        public long id_student_category { get; set; }

        [ForeignKey("Role")]
        public long id_role { get; set; }

        // Navigation property
        public virtual StudentDetails StudentDetails { get; set; }
        public virtual StudentCategory StudentCategory { get; set; }
        public ICollection<StudentFees> StudentFees { get; set; }
        public virtual Role Role { get; set; }
    }
}
