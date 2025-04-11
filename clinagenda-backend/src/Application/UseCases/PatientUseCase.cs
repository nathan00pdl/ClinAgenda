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

        public async Task<PatientResponseDTO> GetAllPatientAsync(String? name, String? documentNumber, int? statusId, int itemsPerPage, int page)
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

            return new PatientResponseDTO
            { 
                Total = total, 
                Items = patients 
            };
        }

        public async Task<PatientListReturnDTO> GetPatientByIdAsync(int id)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(id) ?? throw new KeyNotFoundException($"Patient with ID {id} Not Found");

            var returnPatient = new PatientListReturnDTO
            {
                Id = patient.Id,
                Name = patient.Name,
                PhoneNumber = patient.PhoneNumber,
                DocumentNumber = patient.DocumentNumber,
                BirthDate = patient.BirthDate,
                Status = new StatusDTO
                {
                    Id = patient.StatusId,
                    Name = patient.StatusName
                }
            };

            return returnPatient;
        }

        public async Task<int> CreatePatientAsync(PatientInsertDTO patientInsertDTO) 
        {
            return await _patientRepository.InsertPatientAsync(patientInsertDTO);
        }

        public async Task<bool> UpdatePatientAsync(int id, PatientInsertDTO patientInsertDTO)
        {
            var existingPatient = await _patientRepository.GetPatientByIdAsync(id) ?? throw new KeyNotFoundException($"Patient with ID {id} Not Found");

            if (existingPatient == null) return false;
            
            PatientDTO patientUpdate = new PatientDTO
            {
                Id = id,
                Name = patientInsertDTO.Name,
                PhoneNumber = patientInsertDTO.PhoneNumber,
                DocumentNumber = patientInsertDTO.DocumentNumber,
                BirthDate = patientInsertDTO.BirthDate,
                StatusId = patientInsertDTO.StatusId
            };

            return await _patientRepository.UpdatePatientAsync(patientUpdate);
        }

        public async Task<bool> DeletePatientASync(int id)
        {
            var rowsAffected = await _patientRepository.DeletePatientAsync(id);
            return rowsAffected > 0;
        }

        public async Task<object?> AutoCompletePatientAsync(String name)
        {
            var rawData = await _patientRepository.AutoCompletePatientAsync(name);

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