using ClinAgenda.Application.DTOs.Doctor;

namespace ClinAgenda.Core.Interfaces
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<DoctorListDTO>> GetDoctorsAsync(String? name, int? specialtyId, int? statusId, int offset, int itemsPerPage);
        Task<IEnumerable<DoctorListDTO>> GetDoctorsByIdAsync(int id);
        Task<IEnumerable<SpecialtyDoctorDTO>> GetDoctorSpecialtiesAsync(int[] doctorIds);
        Task<int> InsertDoctorAsync(DoctorInsertDTO doctorInsertDTO);
        Task<bool> UpdateDoctorAsync(DoctorDTO doctorDTO);
        Task<int> DeleteDoctorAsync(int id);
    }
}