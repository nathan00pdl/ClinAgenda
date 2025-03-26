namespace ClinAgenda.Application.DTOs.Doctor
{
    public class DoctorDTO
    {
        private int Id { get; set; }
        public required String Name { get; set; }
        public int StatusId { get; set; }
    }
}