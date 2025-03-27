using ClinAgenda.Application.DTOs.Patient;
using ClinAgenda.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ClinAgenda.WebAPI.Controllers
{
    [ApiController]
    [Route("api/patient")]
    public class PatientController : ControllerBase
    {
        private readonly PatientUseCase _patientUseCase;
        private readonly StatusUseCase _statusUseCase;

        public PatientController(PatientUseCase patientService, StatusUseCase statusService)
        {
            _patientUseCase = patientService;
            _statusUseCase = statusService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllPatient([FromQuery] String? name, [FromQuery] String? documentNumber, [FromQuery] int? statusId, [FromQuery] int itemsPerPage = 10, [FromQuery] int page = 1)
        {
            try
            {
                var result = await _patientUseCase.GetAllPatientAsync(name, documentNumber, statusId, itemsPerPage, page);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("listById/{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            try
            {
                var patient = await _patientUseCase.GetPatientByIdAsync(id);
                if (patient == null) return NotFound();
                
                return Ok(patient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpPost("insert")]
        public async Task<IActionResult> CreatePatient([FromBody] PatientInsertDTO patient)
        {
            try
            {
                var hasStatus = await _statusUseCase.GetStatusByIdAsync(patient.StatusId);
                if (hasStatus == null)
                    return BadRequest($"The Status with ID {patient.StatusId} Dont Exist.");

                var createdPatientId = await _patientUseCase.CreatePatientAsync(patient);
                if (!(createdPatientId > 0))
                {
                    return StatusCode(500, "Error Creating Patient.");
                }

                var infosPatientCreated = await _patientUseCase.GetPatientByIdAsync(createdPatientId);

                return Ok(infosPatientCreated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"={ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdatePatientAsync(int id, [FromBody] PatientInsertDTO patient)
        {
            try
            {
                if (patient == null) return BadRequest();

                var hasStatus = await _statusUseCase.GetStatusByIdAsync(patient.StatusId);
                if (hasStatus == null) return BadRequest($"O status ID {patient.StatusId} não existe");

                bool updated = await _patientUseCase.UpdatePatientAsync(id, patient);
                if (!updated) return NotFound("Patient Not Found.");
                var infosPatientUpdate = await _patientUseCase.GetPatientByIdAsync(id);

                return Ok(infosPatientUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpGet("autocomplete")]
        public async Task<IActionResult> AutoComplete([FromQuery] string? name)
        {
            try
            {
                var result = await _patientUseCase.AutoComplete(name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }
    }
}