using System.ComponentModel.DataAnnotations;
using ClinAgenda.Core.Entities;

namespace ClinAgenda.Application.DTOs.Patient
{
    public class PatientDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Patient's Name is Mandatory", AllowEmptyStrings = false)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "The Patient's Phone Number is Mandatory", AllowEmptyStrings = false)]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The Patient's Document Number is Mandatory", AllowEmptyStrings = false)]
        public required string DocumentNumber { get; set; }

        [Required(ErrorMessage = "The Patient's Status is Mandatory", AllowEmptyStrings = false)]
        public required int StatusId { get; set; }

        [Required(ErrorMessage = "The Patient's Date of Birth is Mandatory", AllowEmptyStrings = false)]
        public required string BirthDate { get; set; }
    }
}