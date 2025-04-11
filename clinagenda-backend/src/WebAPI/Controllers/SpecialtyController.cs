using ClinAgenda.Application.DTOs;
using ClinAgenda.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ClinAgenda.WebAPI.Controllers
{
    [ApiController]
    [Route("api/specialty")]
    public class SpecialtyController : ControllerBase
    {
        private readonly SpecialtyUseCase _specialtyUsecase;
        private readonly DoctorUseCase _doctorUseCase;

        public SpecialtyController(SpecialtyUseCase SpecialtyService, DoctorUseCase doctorService)
        {
            _specialtyUsecase = SpecialtyService;
            _doctorUseCase = doctorService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllSpecialty([FromQuery] int itemsPerPage = 10, [FromQuery] int page = 1)
        {
            try
            {
                var specialty = await _specialtyUsecase.GetAllSpecialtyAsync(itemsPerPage, page);
                return Ok(specialty);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("listById/{id}")]
        public async Task<IActionResult> GetSpecialtyById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid ID.");
                }

                var specialty = await _specialtyUsecase.GetSpecialtyByIdAsync(id);
                if (specialty == null)
                {
                    return NotFound($"Specialty with ID {id} Not Found.");
                }

                return Ok(specialty);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("insert")]
        public async Task<IActionResult> CreateSpecialty([FromBody] SpecialtyInsertDTO specialtyInsertDTO)
        {
            try
            {
                if (specialtyInsertDTO == null)
                {
                    return BadRequest("Invalid Datas for Specialty Creation.");
                }

                var createdSpecialty = await _specialtyUsecase.CreateSpecialtyAsync(specialtyInsertDTO);
                if (!(createdSpecialty > 0))
                {
                    return StatusCode(500, "Error Creating Specialty.");
                }

                var infosSpecialtyCreated = await _specialtyUsecase.GetSpecialtyByIdAsync(createdSpecialty);
                return Ok(infosSpecialtyCreated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateSpecialty(int id, [FromBody] SpecialtyDTO specialtyDTO)
        {
            try
            {
                if (specialtyDTO == null) return BadRequest();

                bool updated = await _specialtyUsecase.UpdateSpecialtyAsync(id, specialtyDTO);
                if (!updated) return NotFound("Specialty Not Found.");

                var updatedSpecialty = await _specialtyUsecase.UpdateSpecialtyAsync(id, specialtyDTO);

                return Ok(updatedSpecialty);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSpecialty(int id)
        {
            try
            {
                var hasDoctor = await _doctorUseCase.GetAllDoctorAsync(null, specialtyId: id, null, 1, 1);
                if (hasDoctor.Total > 0)
                {
                    return StatusCode(500, $"Specialty Associated with One or More Doctors.");
                }

                var success = await _specialtyUsecase.DeleteSpecialtyAsync(id);
                if (!success)
                {
                    return NotFound("Specialty with ID {id} Not Found.");
                }

                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}