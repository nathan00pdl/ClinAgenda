namespace ClinAgenda.Application.DTOs.Patient
{
    public class PatientResponseDTO
    {
        public int Total { get; set; }
        public required List<PatientListReturnDTO> Items { get; set; }
    }
}