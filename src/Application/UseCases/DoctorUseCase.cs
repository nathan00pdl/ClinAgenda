using ClinAgenda.Application.DTOs;
using ClinAgenda.Application.DTOs.Doctor;
using ClinAgenda.Core.Entities;
using ClinAgenda.Core.Interfaces;

namespace ClinAgenda.Application.UseCases
{
    public class DoctorUseCase
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDoctorSpecialtyRepository _doctorSpecialtyRepository;
        private readonly ISpecialtyRepository _specialtyRepository;

        public DoctorUseCase(IDoctorRepository doctorRepository, IDoctorSpecialtyRepository doctorspecialtyRepository, ISpecialtyRepository specialtyRepository)
        {
            _doctorRepository = doctorRepository;
            _doctorSpecialtyRepository = doctorspecialtyRepository;
            _specialtyRepository = specialtyRepository;
        }

        public async Task<object> GetAllDoctorAsync(String? name, int? specialtyId, int? statusId, int itemsPerPage, int page)
        {
            int offset = (page - 1) * itemsPerPage;

            var doctors = (await _doctorRepository.GetAllDoctorAsync(name, specialtyId, statusId, offset, itemsPerPage)).ToList();
            if (!doctors.Any()) return new { total = 0, items = new List<DoctorListReturnDTO>() };

            var doctorIds = doctors.Select(d => d.Id).ToArray();
            
            var specialties = (await _doctorRepository.GetDoctorSpecialtiesAsync(doctorIds)).ToList();

            var result = doctors.Select(d => new DoctorListReturnDTO
            {
                Id = d.Id,
                Name = d.Name,
                Status = new StatusDTO
                    {
                        Id = d.StatusId,
                        Name = d.StatusName
                    },
                Specialty = specialties
                    .Where(s => s.DoctorId == d.Id)
                    .Select(s => new SpecialtyDTO
                        {
                            Id = s.SpecialtyId,
                            Name = s.SpecialtyName,
                            ScheduleDuration = s.ScheduleDuration
                        }
                        ).ToList()
            });

            return new 
            {
                total = result.Count(),
                items = result.ToList()
            };
        }

        public async Task<object> GetDoctorByIdAsync(int id)
        {
            var rawData = await _doctorRepository.GetDoctorByIdAsync(id);

            var result = new
            {
                item = rawData
                    .GroupBy(item => item.Id)
                    .Select(group => new
                    {
                        id = group.Key,
                        name = group.First().Name,
                        specialty = group
                            .Select(s => new
                            {
                                id = s.SpecialtyId,
                                name = s.SpecialtyName
                            })
                            .ToList(),
                        status = new
                        {
                            id = group.First().StatusId,
                            name = group.First().StatusName
                        }
                    })
                .ToList()
            };

            return new
            {
                item = result.item
            };
        }

        public async Task<int> CreateDoctorAsync(DoctorInsertDTO doctorInsertDTO)
        {
            var newDoctorId = await _doctorRepository.InsertDoctorAsync(doctorInsertDTO);

            var doctor_specialities = new DoctorSpecialtyDTO
            {
                DoctorId = newDoctorId,
                SpecialtyId = doctorInsertDTO.Specialty
            };

            await _doctorSpecialtyRepository.InsertDoctorSpecialtyAsync(doctor_specialities);

            return newDoctorId;
        }

        public async Task<bool> UpdateDoctorAsync(int id, DoctorInsertDTO doctorInsertDTO)
        {
            var doctorToUpdate = new DoctorDTO
            {
                Id = id,
                Name = doctorInsertDTO.Name,
                StatusId = doctorInsertDTO.StatusId
            };

            await _doctorRepository.UpdateDoctorAsync(doctorToUpdate);

            await _doctorSpecialtyRepository.DeleteDoctorSpecialtyAsync(id);

            var doctorSpecialties = new DoctorSpecialtyDTO
            {
                DoctorId = id,
                SpecialtyId = doctorInsertDTO.Specialty
            };

            await _doctorSpecialtyRepository.InsertDoctorSpecialtyAsync(doctorSpecialties);

            return true;
        }

        public async Task<bool> DeleteDoctorASync(int id)
        {
            var rowsAffected = await _doctorRepository.DeleteDoctorAsync(id);
            return rowsAffected > 0;
        }
    }
}