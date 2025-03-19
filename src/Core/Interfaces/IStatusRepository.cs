using ClinAgenda.Core.Entities;

namespace ClinAgenda.Core.Interfaces
{
    public interface IStatusRepository
    {
        Task<StatusDTO> GetByIdAsync(int Id);
    }
}