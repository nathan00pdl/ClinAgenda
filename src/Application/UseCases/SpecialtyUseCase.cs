using ClinAgenda.Application.DTOs;
using ClinAgenda.Core.Interfaces;

namespace ClinAgenda.Application.UseCases
{
    public class SpecialtyUseCase
    {
        private readonly ISpecialtyRepository _specialtyRepository;

        public SpecialtyUseCase(ISpecialtyRepository specialtyRepository)
        {
            _specialtyRepository = specialtyRepository;
        }

        public async Task<object> GetSpecialtyAsync(int itemsPerPage, int page)
        {
            var (total, rawData) = await _specialtyRepository.GetAllSpecialtyAsync(itemsPerPage, page);

            return new
            {
                total,
                items = rawData.ToList()
            };
        }

        public async Task<int> CreateSpecialtyAsync(SpecialtyInsertDTO specialtyInsertDTO)
        {
           
            var newSpecialtyId = await _specialtyRepository.InsertSpecialtyAsync(specialtyInsertDTO);

            return newSpecialtyId;

        }
        public async Task<SpecialtyDTO?> GetSpecialtyByIdAsync(int id)
        {
            return await _specialtyRepository.GetSpecialtyByIdAsync(id);
        }
    }
}