using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class StudentFees
    {
        [Key]
        public long id_student_fees { get; set; }

        public string fees_for_month { get; set; }
        public string fees_for_year { get; set; }
        public decimal fee_amount { get; set; }
        public bool feebook_entry_done { get; set; }
        public string created_by_user { get; set; }
        public string last_updated_by_user { get; set; }
        public string deleted_by_user { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        [ForeignKey("Student")]
        public long id_student { get; set; }
        
        // Navigation property for the reference to Student
        public virtual Student Student { get; set; }
    }
}
