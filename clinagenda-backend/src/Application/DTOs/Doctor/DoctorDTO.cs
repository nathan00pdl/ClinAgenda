namespace ClinAgenda.Application.DTOs.Doctor
{
    public class DoctorDTO
    {
        public int Id { get; set; }
        public required String Name { get; set; }
        public int StatusId { get; set; }
    }
}