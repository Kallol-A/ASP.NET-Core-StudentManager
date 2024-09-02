using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DTOs
{
    public class StudentIdDto
    {
        public int StudentId { get; set; }
    }

    public class StudentDetailDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
        public string Role { get; set; }
        public List<StudentFeesDto> Fees { get; set; }
    }

    public class StudentFeesDto
    {
        public string FeesForMonth { get; set; }
        public string FeesForYear { get; set; }
        public decimal FeeAmount { get; set; }
        public bool FeebookEntryDone { get; set; }

    }
}
