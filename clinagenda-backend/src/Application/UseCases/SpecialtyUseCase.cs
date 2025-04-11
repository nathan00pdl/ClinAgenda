using ClinAgenda.Application.DTOs;
using ClinAgenda.Core.Interfaces;
using Org.BouncyCastle.Asn1.Rosstandart;

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
            return await _specialtyRepository.InsertSpecialtyAsync(specialtyInsertDTO);
        }

        public async Task<bool> UpdateSpecialtyAsync(int id, SpecialtyDTO specialtyDTO)
        {
            var existingSpecialty = await _specialtyRepository.GetSpecialtyByIdAsync(id) ?? throw new KeyNotFoundException($"Specialty with ID {id} Not Found");

            existingSpecialty.Name = specialtyDTO.Name;
            existingSpecialty.ScheduleDuration = specialtyDTO.ScheduleDuration;

            return await _specialtyRepository.UpdateSpecialtyAsync(existingSpecialty);
        }

        public async Task<bool> DeleteSpecialtyAsync(int id)
        {
            var rowsAffected = await _specialtyRepository.DeleteSpecialtyAsync(id);
            return rowsAffected > 0;
        }
    }
}