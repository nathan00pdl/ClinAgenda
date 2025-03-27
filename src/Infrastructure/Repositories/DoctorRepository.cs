using ClinAgenda.Application.DTOs.Doctor;
using ClinAgenda.Core.Interfaces;

namespace ClinAgenda.Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {

        public Task<IEnumerable<DoctorListDTO>> GetDoctorsAsync(string? name, int? specialtyId, int? statusId, int offset, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DoctorListDTO>> GetDoctorsByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SpecialtyDoctorDTO>> GetDoctorSpecialtiesAsync(int[] doctorIds)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertDoctorAsync(DoctorInsertDTO doctorInsertDTO)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDoctorAsync(DoctorDTO doctorDTO)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteDoctorAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}