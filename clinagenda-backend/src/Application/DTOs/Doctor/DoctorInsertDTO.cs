namespace ClinAgenda.Application.DTOs.Doctor
{
    public class DoctorInsertDTO
    {
        public required String Name { get; set; }
        public required List<int> Specialty { get; set; }
        public int StatusId { get; set; }
    }
}