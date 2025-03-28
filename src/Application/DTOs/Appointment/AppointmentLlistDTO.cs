namespace ClinAgenda.Application.DTOs.Appointment
{
    public class AppointmentListDTO
    {
        public int Id { get; set; }
        public required String PatientName { get; set; }
        public required String PatientDocument { get; set; }
        public required String DoctorName { get; set; }
        public required int SpecialtyId { get; set; }
        public required String SpecialtyName { get; set; }
        public required String AppointmentDate { get; set; }
        public int ScheduleDuration { get; set; }
    }
}