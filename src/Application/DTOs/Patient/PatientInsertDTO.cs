namespace ClinAgenda.Application.DTOs.Patient
{
    public class PatientInsertDTO
    {
        public required String Name { get; set; }
        public required String PhoneNumber { get; set; }
        public required String DocumentNumber { get; set; }
        public required int StatusId { get; set; }
        public required String BirthDate { get; set; }
    }
}