using ClinAgenda.Application.DTOs;
using ClinAgenda.Application.UseCases;
using ClinAgenda.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp.Unsafe;

namespace ClinAgenda.WebAPI.Controllers
{
    [ApiController]
    [Route("api/status")]
    public class StatusController : ControllerBase
    {
        private readonly StatusUseCase _statusUseCase;
        private readonly PatientUseCase _patientUseCase;
        private readonly DoctorUseCase _doctorUseCase;

        public StatusController(StatusUseCase statusService, PatientUseCase patientService, DoctorUseCase doctorService)
        {
            _statusUseCase = statusService;
            _patientUseCase = patientService;
            _doctorUseCase = doctorService;
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

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            var hasPatients = await _patientUseCase.GetAllPatientAsync(null, null, statusId: id, 1, 1);
            var hasDoctors = await _doctorUseCase.GetAllDoctorAsync(null, null, statusId: id, 1, 1);

            if (hasPatients.Total > 0 || hasDoctors.Total > 0)
            {
                return StatusCode(500, $"The Status is Associated with One or More Patients / Doctors");
            }

            var success = await _statusUseCase.DeleteStatusAsync(id);
            if (!success) 
            {
                return NotFound("Status with ID {id} Not Found.");
            }

            return Ok();
        }
    }
}