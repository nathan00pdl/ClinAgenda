namespace ClinAgenda.Application.DTOs
{
    public class AppointmentDTO
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int SpecialtyId { get; set; }
        public required String AppointmentDate { get; set; }
        public required String Observation { get; set; }
    }
}   