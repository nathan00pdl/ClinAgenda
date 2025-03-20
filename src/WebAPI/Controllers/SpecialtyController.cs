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

        public SpecialtyController(SpecialtyUseCase service)
        {
            _specialtyUsecase = service;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetSpecialtyAsync([FromQuery] int itemsPerPage = 10, [FromQuery] int page = 1)
        {
            try
            {
                var specialty = await _specialtyUsecase.GetSpecialtyAsync(itemsPerPage, page);
                return Ok(specialty);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("insert")]
        public async Task<IActionResult> CreateSpecialtyAsync([FromBody] SpecialtyInsertDTO specialty)
        {
            try
            {
                if (specialty == null)
                {
                    return BadRequest("Invalid Datas for Specialty Creation.");
                }

                var createdSpecialty = await _specialtyUsecase.CreateSpecialtyAsync(specialty);

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

        [HttpGet("listById/{id}")]
        public async Task<IActionResult> GetSpecialtyByIdAsync(int id)
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
    }
}