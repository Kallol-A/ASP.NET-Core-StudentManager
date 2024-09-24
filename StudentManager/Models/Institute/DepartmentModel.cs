using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManager.Models
{
    public class Department
    {
        [Key]
        public long id_department { get; set; }

        public string department_name { get; set; }
        public string department_type { get; set; }
        public string created_by_user { get; set; }
        public string last_updated_by_user { get; set; }
        public string deleted_by_user { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        // Navigation Property
        public virtual FacultyDetails FacultyDetails { get; set; } // One-to-One Relationship with FacultyDetails
    }
}
