namespace ClinAgenda.Core.Entities
{
    public class Appointment
    {
        public required int Id { get; set; }
        public required int PatientId { get; set; }
        public required int DoctorId { get; set; }
        public required int SpecialtyId { get; set; }
        public required DateTime AppointmentDate { get; set; }
        public required String Observation { get; set; }
    }
}