using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Student
    {
        [Key]
        public long id_student { get; set; }

        public long id_student_category { get; set; }
        public long id_role { get; set; }
        public string student_email { get; set; }
        public string student_password { get; set; }
        public string created_by_user { get; set; }
        public string last_updated_by_user { get; set; }
        public string deleted_by_user { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        // Navigation property
        public virtual StudentDetails StudentDetails { get; set; }
    }
}
