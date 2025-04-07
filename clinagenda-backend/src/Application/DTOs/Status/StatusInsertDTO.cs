using System.ComponentModel.DataAnnotations;

namespace ClinAgenda.Application.DTOs
{
    public class StatusInsertDTO
    {
        [Required(ErrorMessage = "The Status's Name is Mandatory", AllowEmptyStrings = false)]
        public required string Name { get; set; }
    }
}