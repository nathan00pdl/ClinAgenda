using ClinAgenda.Application.DTOs.Doctor;
using ClinAgenda.Application.DTOs.Patient;

namespace ClinAgenda.Application.DTOs.Appointment
{
    public class AppointmentListReturnDTO
    {
        public int Id { get; set; }
        public required PatientReturnAppointmentDTO Patient { get; set; }
        public required DoctorReturnAppointmentDTO Doctor { get; set; }
        public required SpecialtyDTO Specialty { get; set; }
        public required string AppointmentDate { get; set; }
    }
}