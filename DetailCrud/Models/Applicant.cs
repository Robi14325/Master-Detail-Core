using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DetailCrud.Models
{
    public class Applicant
    {
        public Applicant()
        {
            ApplicantExes = new List<ApplicantEx>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; }
        [ValidateNever]
        public string Picpath { get; set; }
        [NotMapped]
        public IFormFile Picture { get; set; }

        public List<ApplicantEx> ApplicantExes { get; set; }


    }
}
