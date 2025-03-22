using ClinAgenda.Application.DTOs.Patient;

namespace ClinAgenda.Core.Interfaces
{
    public interface IPatientRepository
    {
        Task<PatientDTO?> GetByIdAsync(int id);
        Task<int> InsertPatientAsync(PatientInsertDTO patient);
        Task<bool> UpdatePatientAsync(PatientDTO patient);
        Task<int> DeleteByPatientIdAsync(int patientId);
        Task<(int total, IEnumerable<PatientListDTO> patient)> GetPatientsAsync(string? name, string? documentNumber, int? statusId, int itemsPerPage, int page);
    }
}