using ClinAgenda.Application.DTOs;
using ClinAgenda.Application.DTOs.Appointment;
using ClinAgenda.Application.DTOs.Doctor;
using ClinAgenda.Application.DTOs.Patient;
using ClinAgenda.Core.Interfaces;

namespace ClinAgenda.Application.UseCases
{
    public class AppointmentUseCase
    {

        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentUseCase(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<AppointmentResponseDTO> GetAllAppointmentAsyncGetAllppointmentAync(string? patientName, string? doctorName, int? specialtyId, int itemsPerPage, int page)
        {

            var (total, rawData) = await _appointmentRepository.GetAllppointmentAync(patientName, doctorName, specialtyId, itemsPerPage, page);
            var appointmentMap = new Dictionary<int, AppointmentListReturnDTO>();

            foreach (var item in rawData)
            {
                if(!appointmentMap.ContainsKey(item.Id))
                {
                    appointmentMap[item.Id] = new AppointmentListReturnDTO
                    {
                        Id = item.Id,
                        Patient = new PatientReturnAppointmentDTO
                        {
                            Name = item.PatientName,
                            documentNumber = item.PatientDocument
                        },
                        Doctor = new DoctorReturnAppointmentDTO
                        {
                            Name = item.DoctorName
                        },
                        Specialty = new SpecialtyDTO
                        {
                            Id = item.SpecialtyId,
                            Name = item.SpecialtyName
                        },
                        AppointmentDate = item.AppointmentDate
                    };
                }
            }

            return new AppointmentResponseDTO 
            {
                Total = total,
                Items = appointmentMap.Values.ToList()
            };
        }

        public async Task<AppointmentDTO> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(id) ?? throw new InvalidOperationException($"Appointment with ID {id} was Not Found.");
            return appointment;
        }

        public async Task<int> CreateAppointmentAsync(AppointmentDTO appointmentDTO)
        {
            return await _appointmentRepository.InsertAppointmentAsync(appointmentDTO);
        }

        public async Task<bool> UpdateAppointmentAsync(int appointmentId, AppointmentDTO appointmentDTO)
        {
            var updatedPatient = new AppointmentInsertDTO
            {
                Id = appointmentId,
                PatientId = appointmentDTO.PatientId,
                DoctorId = appointmentDTO.DoctorId,
                SpecialtyId = appointmentDTO.SpecialtyId,
                AppointmentDate = appointmentDTO.AppointmentDate,
                Observation = appointmentDTO.Observation
            };

            return await _appointmentRepository.UpdateAppointmentAsync(updatedPatient);
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            var rowsAffected = await _appointmentRepository.DeleteAppointmentAsync(id);
            return rowsAffected > 0;
        }
    }
}