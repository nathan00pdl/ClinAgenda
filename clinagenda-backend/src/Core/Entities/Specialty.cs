namespace ClinAgenda.Core.Entities
{
    public class Specialty
    {
        public int Id { get; set; }
        public required String Name { get; set; }
        public int ScheduleDuration { get; set; }
    }
}