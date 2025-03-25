namespace ClinAgenda.Application.DTOs

/*
 * Study Note
 *
 * When an attribute is marked as "required", it means that it must be initialized before the object is created.
 */

{
    public class StatusInsertDTO
    {
        public required string Name { get; set; }
    }
}