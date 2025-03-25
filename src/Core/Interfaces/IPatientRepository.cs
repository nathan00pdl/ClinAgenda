using ClinAgenda.Application.DTOs.Patient;

namespace ClinAgenda.Core.Interfaces
{
    public interface IPatientRepository
    {
        Task<(int total, IEnumerable<PatientListDTO> patient)> GetPatientsAsync(String? name, String? documentNumber, int? statusId, int itemsPerPage, int page);
        Task<PatientDTO?> GetByIdAsync(int id);
        Task<int> InsertPatientAsync(PatientInsertDTO patient);
        Task<bool> UpdatePatientAsync(PatientDTO patient);
        Task<int> DeleteByPatientIdAsync(int patientId);
        Task<IEnumerable<PatientListDTO>> AutoComplete(String name);
    }
}