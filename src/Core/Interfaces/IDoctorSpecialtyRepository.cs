using ClinAgenda.Application.DTOs.Doctor;

namespace ClinAgenda.Core.Interfaces
{
    public interface IDoctorSpecialtyRepository
    {
        Task InsertDoctorSpecialtyAsync(DoctorSpecialtyDTO doctorSpecialtyDTO); 
        Task DeleteDoctorSpecialtyAsync(int doctorId);
    }
}