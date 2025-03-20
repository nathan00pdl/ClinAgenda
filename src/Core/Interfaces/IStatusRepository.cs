using ClinAgenda.Application.DTOs;
using ClinAgenda.Core.Entities;

namespace ClinAgenda.Core.Interfaces
{
    public interface IStatusRepository
    {
        Task<StatusDTO> GetByIdAsync(int Id);
        Task<int> DeleteStatusAsync(int id);
        Task<int> InsertStatusAsync(StatusInsertDTO statusInsertDTO);
        Task<(int total, IEnumerable<StatusDTO> specialtys)> GetAllAsync(int? itemsPerPage, int? page);
    }
}