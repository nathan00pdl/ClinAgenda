namespace ClinAgenda.Application.DTOs.Appointment
{
    public class AppointmentResponseDTO
    {
        public int Total { get; set; }
        public required List<AppointmentListReturnDTO> Items { get; set; }
    }
}