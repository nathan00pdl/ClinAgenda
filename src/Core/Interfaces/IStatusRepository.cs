using ClinAgenda.Application.DTOs;
using ClinAgenda.Core.Entities;

/*
 * Note:
 *  
 * Task<T> is a generic object that encapsulates a return value of type T.
 * Represents an asynchronous operation that may be in progess, completed or in error.
 * Improves application performance and responsiveness.
 * Avoids unnecessary blocking in long-running calls such as data base access.
 */

namespace ClinAgenda.Core.Interfaces
{
    public interface IStatusRepository
    {
        Task<StatusDTO> GetByIdAsync(int Id);
        Task<int> InsertStatusAsync(StatusInsertDTO statusInsertDTO);
        Task<int> DeleteStatusAsync(int id);
        Task<(int total, IEnumerable<StatusDTO> specialtys)> GetAllAsync(int? itemsPerPage, int? page);
    }
}