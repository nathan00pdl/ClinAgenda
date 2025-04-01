using System.ComponentModel.DataAnnotations;

namespace ClinAgenda.Application.DTOs.Patient
{
    public class PatientInsertDTO
    {
        [Required(ErrorMessage = "The Patient's Name is Mandatory", AllowEmptyStrings = false)]
        public required String @Name { get; set; }

        [Required(ErrorMessage = "The Patient's Phone Number is Mandatory", AllowEmptyStrings = false)]
        public required String PhoneNumber { get; set; }

        [Required(ErrorMessage = "The Patient's Document Number is Mandatory", AllowEmptyStrings = false)]
        public required String DocumentNumber { get; set; }

        [Required(ErrorMessage = "The Patient's Status is Mandatory", AllowEmptyStrings = false)]
        public required int StatusId { get; set; }

        [Required(ErrorMessage = "The Patient's Date of Birth is Mandatory", AllowEmptyStrings = false)]
        public required String BirthDate { get; set; }
    }
}