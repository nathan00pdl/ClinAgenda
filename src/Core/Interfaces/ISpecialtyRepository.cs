using ClinAgenda.Application.DTOs;

namespace ClinAgenda.Core.Interfaces
{
    public interface ISpecialtyRepository
    {
        Task<(int total, IEnumerable<SpecialtyDTO> specialtys)> GetAllSpecialtiesAsync(int? itemsPerPage, int? page);
        Task<SpecialtyDTO> GetSpecialtyByIdAsync(int id);
        Task<int> InsertSpecialtyAsync(SpecialtyInsertDTO specialtyInsertDTO);
        Task<int> DeleteSpecialtyAsync(int id);
    }
}