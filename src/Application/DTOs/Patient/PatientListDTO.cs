using ClinAgenda.Core.Entities;

namespace ClinAgenda.Application.DTOs.Patient
{
    public class PatientListDTO
    {
        public int Id { get; set; }
        public required String Name { get; set; }
        public required String PhoneNumber { get; set; }
        public required String DocumentNumber { get; set; }
        public int StatusId { get; set; }
        public required String StatusName { get; set; }
        public required String BirthDate { get; set; }
    }
}