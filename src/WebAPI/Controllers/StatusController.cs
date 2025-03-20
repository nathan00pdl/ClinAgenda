using ClinAgenda.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ClinAgenda.WebAPI.Controllers
{
    [ApiController]
    [Route("api/status")]
    public class StatusController : ControllerBase
    {
        private readonly StatusUseCase _statusUseCase;

        public StatusController(StatusUseCase service)
        {
            _statusUseCase = service;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetStatusAsync([FromQuery] int itemsPerPage = 10, [FromQuery] int page = 1)
        {
            try
            {
                var specialty = await _statusUseCase.GetStatusAsync(itemsPerPage, page);
                return Ok(specialty);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error When Fetching Status: {ex.Message}");
            }
        }

        [HttpGet("ListById/{id}")]
        public async Task<IActionResult> GetStatusByIdAsync(int id)
        {
            try
            {
                var specialty = await _statusUseCase.GetStatusByIdAsync(id);

                if (specialty == null) 
                {
                    return NotFound($"Status with ID {id} not found.");
                }

                return Ok(specialty);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error When Fetching Status By ID: {ex.Message}");
            }
        }
    }
}