using ClinAgenda.Application.DTOs.Patient;

namespace ClinAgenda.Core.Interfaces
{
    public interface IPatientRepository
    {
        Task<(int total, IEnumerable<PatientListDTO> patient)> GetAllPatientAsync(String? name, String? documentNumber, int? statusId, int itemsPerPage, int page);
        Task<PatientListDTO?> GetPatientByIdAsync(int id);
        Task<int> InsertPatientAsync(PatientInsertDTO patientInsertDTO);
        Task<bool> UpdatePatientAsync(PatientDTO patientDTO);
        Task<int> DeletePatientAsync(int patientId);
        Task<IEnumerable<PatientListDTO>> AutoCompletePatientAsync(String name);
    }
}