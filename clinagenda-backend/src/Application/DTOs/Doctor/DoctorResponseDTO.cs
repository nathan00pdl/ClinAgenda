namespace ClinAgenda.Application.DTOs.Doctor
{
    public class DoctorResponseDTO
    {
        public int Total { get; set; }
        public required List<DoctorListReturnDTO> Items { get; set; }
    }
}