namespace ClinAgenda.Application.DTOs
{
    public class SpecialtyDTO
    {
        public int Id { get; set; }
        public required String Name { get; set; }
        public int ScheduleDuration { get; set; }
    }
}