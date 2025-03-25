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
        private readonly StatusUseCase _StatusUseCase;

        public StatusController(StatusUseCase service)
        {
            _StatusUseCase = service;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetStatusAsync([FromQuery] int itemsPerPage = 10, [FromQuery] int page = 1)
        {
            try
            {
                var (total, rawData) = await _StatusUseCase.GetStatusAsync(itemsPerPage, page);

                return ok(new
                {
                    total,
                    itemsPerPage,
                    page,
                    items = rawData.ToList()
                });
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
                var specialty = await _StatusUseCase.GetStatusByIdAsync(id);

                if (specialty == null)
                {
                    return NotFound(new
                    {
                        message = $"Status with ID {id} Not Found."
                    });
                }

                return Ok(new
                {
                    message = "Status Not Found",
                    status = specialty
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error When Fetching Status By ID: {ex.Message}");
            }
        }

        [HttpPost("insert")]
        public async Task<IActionResult> CreateStatusAsync([FromBody] StatusInsertDTO statusInsertDTO)
        {
            try
            {
                if (statusInsertDTO == null || string.IsNullOrWhiteSpace(statusInsertDTO.Name))
                {
                    return BadRequest($"Status Data is Invalid.");
                }

                var createdStatus = await _StatusUseCase.CreateStatusAsync(statusInsertDTO);
                var InfosStatusCreated = await _StatusUseCase.GetStatusByIdAsync(createdStatus);

                return CreatedAtAction(nameof(GetStatusByIdAsync), new {id = createdStatus}, InfosStatusCreated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error When Created Status: {ex.Message}");
            }
        }
    }
}