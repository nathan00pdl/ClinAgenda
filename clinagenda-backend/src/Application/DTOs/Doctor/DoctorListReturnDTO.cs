using ClinAgenda.Core.Entities;

namespace ClinAgenda.Application.DTOs.Doctor
{
    public class DoctorListReturnDTO
    {
        public required int Id { get; set; }
        public required String Name { get; set; }
        public required List<SpecialtyDTO> Specialty { get; set; }
        public required StatusDTO Status { get; set; }
    }
}