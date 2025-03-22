using ClinAgenda.Core.Entities;

namespace ClinAgenda.Application.DTOs.Patient
{
    public class PatientListReturnDTO
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        public required string DocumentNumber { get; set; }
        public required string BirthDate { get; set; }
        public required StatusDTO Status { get; set; }
    }
}