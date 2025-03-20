using ClinAgenda.Application.DTOs;
using ClinAgenda.Application.UseCases;
using ClinAgenda.Core.Entities;
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

        [HttpGet("list")]
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

        [HttpGet("listById/{id}")]
        public async Task<IActionResult> GetStatusByIdAsync(int id)
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
        public async Task<IActionResult> CreateStatusAsync([FromBody] StatusInsertDTO status)
        {
            try
            {
                if (status == null || string.IsNullOrWhiteSpace(status.Name))
                {
                    return BadRequest($"Status Data is Invalid.");
                }

                var createdStatus = await _statusUseCase.CreateStatusAsync(status);
                var InfosStatusCreated = await _statusUseCase.GetStatusByIdAsync(createdStatus);

                return CreatedAtAction(nameof(GetStatusByIdAsync), new {id = createdStatus}, InfosStatusCreated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error When Created Status: {ex.Message}");
            }
        }
    }
}