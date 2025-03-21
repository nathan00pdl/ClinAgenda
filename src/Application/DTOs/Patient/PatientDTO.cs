using ClinAgenda.Core.Entities;

namespace ClinAgenda.Application.DTOs.Patient
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public required String Name { get; set; }
        public required String PhoneNumber { get; set; }
        public required String DocumentNumber { get; set; }
        public required StatusDTO Status { get; set; }
        public required String BirthDate { get; set; }
    }
}