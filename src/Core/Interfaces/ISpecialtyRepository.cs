using ClinAgenda.Application.DTOs;

namespace ClinAgenda.Core.Interfaces
{
    public interface ISpecialtyRepository
    {
        Task<SpecialtyDTO> GetByIdAsync(int Id);
        Task<int> DeleteStatusAsync(int id);
        Task<int> InsertStatusAsync(StatusInsertDTO statusInsertDTO);
        Task<(int total, IEnumerable<SpecialtyDTO> specialtys)> GetAllAsync(int? itemsPerPage, int? page);
    }
}