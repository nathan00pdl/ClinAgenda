using ClinAgenda.Application.DTOs.Patient;
using ClinAgenda.Core.Entities;
using ClinAgenda.Core.Interfaces;

namespace ClinAgenda.Application.UseCases
{
    public class PatientUseCase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientUseCase(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<object> GetAllPatientAsync(String? name, String? documentNumber, int? statusId, int itemsPerPage, int page)
        {
            var (total, rawData) = await _patientRepository.GetAllPatientAsync(name, documentNumber, statusId, itemsPerPage, page);

            var patients = rawData
                .Select(p => new PatientListReturnDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    PhoneNumber = p.PhoneNumber,
                    DocumentNumber = p.DocumentNumber,
                    BirthDate = p.BirthDate,
                    Status = new StatusDTO
                    {
                        Id = p.StatusId,
                        Name = p.StatusName
                    }
                })
                .ToList();

            return new { total, items = patients };
        }

        public async Task<PatientDTO> GetPatientByIdAsync(int id)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(id) ?? throw new KeyNotFoundException("Patient Not Found");
            return patient;
        }

        public async Task<bool> UpdatePatientAsync(int patientId, PatientInsertDTO patientInsertDTO)
        {
            var existingPatient = await _patientRepository.GetPatientByIdAsync(patientId) ?? throw new KeyNotFoundException("Patient Not Found");

            existingPatient.Name = patientInsertDTO.Name;
            existingPatient.PhoneNumber = patientInsertDTO.PhoneNumber;
            existingPatient.DocumentNumber = patientInsertDTO.DocumentNumber;
            existingPatient.StatusId = patientInsertDTO.StatusId;
            existingPatient.BirthDate = patientInsertDTO.BirthDate;

            var isUpdated = await _patientRepository.UpdatePatientAsync(existingPatient);

            return isUpdated;
        }

        public async Task<object?> AutoCompletePatient(String name)
        {
            var rawData = await _patientRepository.AutoCompletePatient(name);

            var patients = rawData
                .Select(p => new PatientListReturnDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    PhoneNumber = p.PhoneNumber,
                    DocumentNumber = p.DocumentNumber,
                    BirthDate = p.BirthDate,
                    Status = new StatusDTO
                    {
                        Id = p.StatusId,
                        Name = p.StatusName
                    }
                })
                .ToList();
            
            return patients;
        } 
    }
}