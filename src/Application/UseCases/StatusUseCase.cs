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

        public async Task<object> GetAllStatusAsync(int itemsPerPage, int page)
        {
            var (total, rawData) = await _statusRepository.GetAllStatusAsync(itemsPerPage, page);

            return new
            {
                total,
                items = rawData.ToList()
            };
        }

        public async Task<StatusDTO?> GetStatusByIdAsync(int id)
        {
            return await _statusRepository.GetStatusByIdAsync(id);
        }

        public async Task<int> CreateStatusAsync(StatusInsertDTO statusInsertDTO)
        {
            var newStatusId = await _statusRepository.InsertStatusAsync(statusInsertDTO );

            return newStatusId; 
        }
    }
}