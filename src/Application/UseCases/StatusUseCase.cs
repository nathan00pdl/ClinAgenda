using ClinAgenda.Application.DTOs;
using ClinAgenda.Core.Entities;
using ClinAgenda.Core.Interfaces;

namespace ClinAgenda.Application.UseCases
{
    public class StatusUseCase
    {
        private readonly IStatusRepository _IstatusRepository;

        public StatusUseCase(IStatusRepository statusRepository)
        {
            _IstatusRepository = statusRepository; 
        }

        public async Task<object> GetStatusAsync(int itemsPerPage, int page)
        {
            var (total, rawData) = await _IstatusRepository.GetAllAsync(itemsPerPage, page);

            return new
            {
                total,
                items = rawData.ToList()
            };
        }

        public async Task<StatusDTO?> GetStatusByIdAsync(int id)
        {
            return await _IstatusRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateStatusAsync(StatusInsertDTO statusInsertDTO)
        {
            var newStatusId = await _IstatusRepository.InsertStatusAsync(statusInsertDTO );

            return newStatusId; 
        }
    }
}