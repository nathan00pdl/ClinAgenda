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

        public async Task<object> GetAllSpecialtyAsync(int itemsPerPage, int page)
        {
            var (total, rawData) = await _specialtyRepository.GetAllSpecialtyAsync(itemsPerPage, page);

            return new
            {
                total,
                items = rawData.ToList()
            };
        }

        public async Task<SpecialtyDTO?> GetSpecialtyByIdAsync(int id)
        {
            return await _specialtyRepository.GetSpecialtyByIdAsync(id);
        }

        public async Task<IEnumerable<SpecialtyDTO>> GetSpecialtiesByIds(List<int> id)
        {
            return await _specialtyRepository.GetSpecialtiesByIds(id);
        }

        public async Task<int> CreateSpecialtyAsync(SpecialtyInsertDTO specialtyInsertDTO)
        {
           
            var newSpecialtyId = await _specialtyRepository.InsertSpecialtyAsync(specialtyInsertDTO);

            return newSpecialtyId;

        }
    }
}