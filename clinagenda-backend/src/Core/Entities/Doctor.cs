namespace ClinAgenda.Core.Entities
{
    public class Doctor
    {
        private int Id { get; set; }
        public required String Name { get; set; }
        public int StatusId { get; set; }
    }
}