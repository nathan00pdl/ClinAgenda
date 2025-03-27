using ClinAgenda.Application.DTOs.Doctor;

namespace ClinAgenda.Core.Interfaces
{
    public interface IDoctorSpecialtyRepository
    {
        Task InsertAsync(DoctorSpecialtyDTO doctorSpecialtyDTO); 
        Task DeleteAsync(int doctorId);
    }
}