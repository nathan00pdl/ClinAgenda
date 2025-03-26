namespace ClinAgenda.Application.DTOs.Doctor
{
    public class DoctorSpecialtyDTO
    {
        public int DoctorId { get; set; }
        public required List<int> SpecialtyId { get; set; }
    }
}