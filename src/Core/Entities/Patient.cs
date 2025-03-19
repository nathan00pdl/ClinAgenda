using System.Runtime.CompilerServices;

namespace ClinAgenda.Core.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public required string name { get; set; }
        public required string PhoneNumber { get; set; }
        public required string DocumentNumber { get; set; }
        public required int StatusId { get; set; }
    }
}