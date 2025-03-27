namespace ClinAgenda.Application.DTOs.Doctor
{
    public class SpecialtyDoctorDTO
    {
        public int DoctorId { get; set; }
        public int SpecialtyId { get; set; }
        public required String SpecialtyName { get; set; }
        public int ScheduleDuration { get; set; }
    }
}