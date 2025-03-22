using ClinAgenda.Application.DTOs;

namespace ClinAgenda.Core.Interfaces
{
    public interface ISpecialtyRepository
    {
        Task<SpecialtyDTO> GetByIdAsync(int id);
        Task<int> InsertSpecialtyAsync(SpecialtyInsertDTO specialtyInsertDTO);
        Task<int> DeleteSpecialtyAsync(int id);
        Task<(int total, IEnumerable<SpecialtyDTO> specialtys)> GetAllAsync(int? itemsPerPage, int? page);
    }
}