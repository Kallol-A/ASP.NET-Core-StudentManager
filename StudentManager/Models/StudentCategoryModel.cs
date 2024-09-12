using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManager.Models
{
    public class StudentCategory
    {
        [Key]
        public long id_student_category { get; set; }

        public string student_category_name { get; set; }
        public string created_by_user { get; set; }
        public string last_updated_by_user { get; set; }
        public string deleted_by_user { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        // Navigation Property
        public virtual StudentDetails StudentDetails { get; set; } // One-to-One Relationship with StudentDetails
    }
}
