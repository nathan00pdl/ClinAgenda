using ClinAgenda.Application.DTOs;
using ClinAgenda.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ClinAgenda.WebAPI.Controllers
{
    [ApiController]
    [Route("api/status")]
    public class StatusController : ControllerBase
    {
        private readonly StatusUseCase _statusUseCase;

        public StatusController(StatusUseCase statusService)
        {
            _statusUseCase = statusService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllStatus([FromQuery] int itemsPerPage = 10, [FromQuery] int page = 1)
        {
            try
            {
                var specialty = await _statusUseCase.GetAllStatusAsync(itemsPerPage, page);
                return Ok(specialty);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error When Fetching Status: {ex.Message}");
            }
        }

        [HttpGet("listById/{id}")]
        public async Task<IActionResult> GetStatusById(int id)
        {
            try
            {
                var specialty = await _statusUseCase.GetStatusByIdAsync(id);
                if (specialty == null)
                {
                    return NotFound($"Status with ID {id} Not Found.");
                }

                return Ok(specialty);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error When Fetching Status By ID: {ex.Message}");
            }
        }

        [HttpPost("insert")]
        public async Task<IActionResult> CreateStatus([FromBody] StatusInsertDTO statusInsertDTO)
        {
            try
            {
                if (statusInsertDTO == null || string.IsNullOrWhiteSpace(statusInsertDTO.Name))
                {
                    return BadRequest($"Status Data is Invalid.");
                }

                var createdStatus = await _statusUseCase.CreateStatusAsync(statusInsertDTO);
                var InfosStatusCreated = await _statusUseCase.GetStatusByIdAsync(createdStatus);

                return Ok(InfosStatusCreated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error When Created Status: {ex.Message}");
            }
        }
    }
}