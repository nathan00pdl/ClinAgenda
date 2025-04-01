namespace ClinAgenda.Core.Entities

/*
 * Note:
 *
 * When an attribute is marked as "required", it means that it must be initialized before the object is created.
 */

{
    public class Status
    {
        public int Id { get; set; }
        public required String Name { get; set; }
    }
}