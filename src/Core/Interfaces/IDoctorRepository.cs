namespace ClinAgenda.Core.Interfaces
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<DoctorListDTO>> GetDoctorsAsync(String? name, int? specialtyId, int? statusId, int offset, int pageSize);
        Task<IEnumerable<DoctorListDTO>> GetDoctorsByIdAsync(int id);
        Task<IEnumerable<SpecialtyDoctorDTO>> GetDoctorSpecialtiesAsync(int[] doctorIds);
        Task<int> InsertDoctorAsync(DoctorInsertDTO doctorInsertDTO);
        Task<bool> UpdateDoctorAsync(DoctorDTO doctorDTO)
        Task<int> DeleteByDoctorIdAsync(int id);
    }
}