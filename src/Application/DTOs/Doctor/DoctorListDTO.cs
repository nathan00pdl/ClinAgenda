namespace ClinAgenda.Application.DTOs.Doctor
{
    public class DoctorListDTO
    {
        private int Id { get; set; }
        public required String Name { get; set; }
        public required int StatusId { get; set; }
        public required String StatusName { get; set; }
        public int SpecialtyId { get; set; }
        public required String SpecialtyName { get; set; }
        public int ScheduleDuration { get; set; }
    }
}