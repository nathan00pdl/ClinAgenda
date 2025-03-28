using ClinAgenda.Application.DTOs;
using ClinAgenda.Application.DTOs.Appointment;
using ClinAgenda.Core.Interfaces;

namespace ClinAgenda.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        public Task<(int total, IEnumerable<AppointmentListDTO> appointment)> GetAppointmentAync(string? patientName, string? doctorName, int? specialtyId, int itemsPerPage, int page)
        {
            throw new NotImplementedException();
        }

        public Task<AppointmentDTO?> GetAppointmentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAppointmentAsync(AppointmentDTO appointmentDTO)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAppointmentAsync(AppointmentInsertDTO appointmentInsertDTO)
        {
            throw new NotImplementedException();
        }
    }
}