using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace DetailCrud.Models
{
    public class ApplicantEx
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Designation { get; set; }
        public int YearofEx { get; set; }
        [ForeignKey("Applicant")]
        public int ApplicantID { get; set; }
        [ValidateNever]
        public Applicant Applicant { get; set; }
    }
}
