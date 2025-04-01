using ClinAgenda.Application.DTOs;
using ClinAgenda.Application.DTOs.Appointment;

namespace ClinAgenda.Core.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<(int total, IEnumerable<AppointmentListDTO> appointment)> GetAllAppointmentAync(String? patientName, String? doctorName, int? specialtyId, int itemsPerPage, int page);
        Task<AppointmentDTO?> GetAppointmentByIdAsync(int id);  
        Task<int> InsertAppointmentAsync(AppointmentDTO appointmentDTO);
        Task<bool> UpdateAppointmentAsync(AppointmentInsertDTO appointmentInsertDTO);
        Task<int> DeleteAppointmentAsync(int id);
    }
}