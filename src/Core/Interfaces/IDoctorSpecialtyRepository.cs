using ClinAgenda.Application.DTOs.Doctor;

namespace ClinAgenda.Core.Interfaces
{
    public interface IDoctorSpecialtyRepository
    {
        Task <int> InsertDoctorSpecialtyAsync(DoctorSpecialtyDTO doctorSpecialtyDTO); 
        Task <int> DeleteDoctorSpecialtyAsync(int id);
    }
}