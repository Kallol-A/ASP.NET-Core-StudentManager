using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class StudentDetails
    {
        [Key]
        public long id_student_details { get; set; }

        [Required]
        [MaxLength(50)]
        public string student_first_name { get; set; }

        [MaxLength(50)]
        public string student_middle_name { get; set; }

        [Required]
        [MaxLength(50)]
        public string student_last_name { get; set; }

        public string student_address1 { get; set; }
        public string student_address2 { get; set; }
        public string student_city { get; set; }
        public string student_district { get; set; }
        public string student_state { get; set; }
        public string student_pin { get; set; }
        public bool student_feebook_given { get; set; }
        public string created_by_user { get; set; }
        public string last_updated_by_user { get; set; }
        public string deleted_by_user { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        [ForeignKey("Student")]
        public long id_student { get; set; }

        // Navigation property
        public virtual Student Student { get; set; }
    }
}
