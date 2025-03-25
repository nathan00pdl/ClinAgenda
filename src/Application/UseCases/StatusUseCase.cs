using ClinAgenda.Application.DTOs;
using ClinAgenda.Core.Entities;
using ClinAgenda.Core.Interfaces;

namespace ClinAgenda.Application.UseCases
{
    public class StatusUseCase
    {
        private readonly IStatusRepository _statusRepository;

        public StatusUseCase(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository; 
        }

        public async Task<object> GetStatusAsync(int itemsPerPage, int page)
        {
            var (total, rawData) = await _statusRepository.GetAllAsync(itemsPerPage, page);

            return new
            {
                total,
                items = rawData.ToList()
            };
        }

        public async Task<StatusDTO?> GetStatusByIdAsync(int id)
        {
            return await _statusRepository.GetByIdAsync(id);
        }
        
        public async Task<int> CreateStatusAsync(StatusInsertDTO statusDTO)
        {
            var status = new StatusInsertDTO
            {
                Name = statusDTO.Name
            };

            var newStatusId = await _statusRepository.InsertStatusAsync(status);

            return newStatusId; 
        }
    }
}