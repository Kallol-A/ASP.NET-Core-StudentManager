using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManager.Models
{
    public class Faculty
    {
    }

    public class FacultyDetails
    {
        [Key]
        public long id_faculty_details { get; set; }

        [Required]
        public string faculty_type { get; set; }

        [Required]
        [MaxLength(50)]
        public string faculty_first_name { get; set; }

        [MaxLength(50)]
        public string faculty_middle_name { get; set; }

        [Required]
        [MaxLength(50)]
        public string faculty_last_name { get; set; }

        public string faculty_address1 { get; set; }
        public string faculty_address2 { get; set; }
        public string faculty_city { get; set; }
        public string faculty_district { get; set; }
        public string faculty_state { get; set; }
        public string faculty_pin { get; set; }
        public bool faculty_probationary_over { get; set; }
        public string created_by_user { get; set; }
        public string last_updated_by_user { get; set; }
        public string deleted_by_user { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        [ForeignKey("User")]
        public long id_user { get; set; }

        [ForeignKey("Department")]
        public long id_department { get; set; }

        // Navigation property
        public virtual User Faculty { get; set; }
        public virtual Department Department { get; set; }
    }
}
