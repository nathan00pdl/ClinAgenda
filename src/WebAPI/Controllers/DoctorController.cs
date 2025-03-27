using ClinAgenda.Application.DTOs.Doctor;
using ClinAgenda.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ClinAgenda.WebAPI.Controllers
{
    [ApiController]
    [Route("api/doctor")]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorUseCase _doctorUseCase;
        private readonly StatusUseCase _statusUseCase;
        private readonly SpecialtyUseCase _specialtyUseCase;

        public DoctorController(DoctorUseCase doctorUseCase, StatusUseCase statusUseCase, SpecialtyUseCase specialtyUseCase)
        {
            _doctorUseCase = doctorUseCase;
            _statusUseCase = statusUseCase;
            _specialtyUseCase = specialtyUseCase;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetDoctor([FromQuery] string? name, [FromQuery] int? specialtyId, [FromQuery] int? statusId, [FromQuery] int itemsPerPage = 10, [FromQuery] int page = 1)
        {
            try
            {
                var result = await _doctorUseCase.GetDoctorAsync(name, specialtyId, statusId, itemsPerPage, page);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("listById/{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _doctorUseCase.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound();
            return Ok(doctor);
        }
        
        [HttpPost("insert")]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorInsertDTO doctorInsertDTO)
        {
            try
            {
                var hasStatus = await _statusUseCase.GetStatusByIdAsync(doctorInsertDTO.StatusId);
                if (hasStatus == null)
                    return BadRequest($"The Status with ID {doctorInsertDTO.StatusId} Does Not Exist.");

                var specialties = await _specialtyUseCase.GetSpecialtiesByIds(doctorInsertDTO.Specialty);

                var notFoundSpecialties = doctorInsertDTO.Specialty.Except(specialties.Select(s => s.Id)).ToList();
                if (notFoundSpecialties.Any())
                {
                    return BadRequest(notFoundSpecialties.Count > 1 ? $"The Specialties with IDs {string.Join(", ", notFoundSpecialties)} Does Not Exist." : $"The Specialty with ID {notFoundSpecialties.First().ToString()} Does Not Exist.");
                }

                var createdDoctorId = await _doctorUseCase.CreateDoctorAsync(doctorInsertDTO);

                var ifosDoctor = await _doctorUseCase.GetDoctorByIdAsync(createdDoctorId);

                return Ok(ifosDoctor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateDoctorAsync(int id, [FromBody] DoctorInsertDTO doctorInsertDTO)
        {
            if (doctorInsertDTO == null) return BadRequest();

            var hasStatus = await _statusUseCase.GetStatusByIdAsync(doctorInsertDTO.StatusId);
            if (hasStatus == null)
                return BadRequest($"O status com ID {doctorInsertDTO.StatusId} não existe.");

            var specialties = await _specialtyUseCase.GetSpecialtiesByIds(doctorInsertDTO.Specialty);

            var notFoundSpecialties = doctorInsertDTO.Specialty.Except(specialties.Select(s => s.Id)).ToList();

            if (notFoundSpecialties.Any())
            {
                return BadRequest(notFoundSpecialties.Count > 1 ? $"The Specialties with IDs {string.Join(", ", notFoundSpecialties)} Does Not Exist" : $"The Specialty with ID {notFoundSpecialties.First().ToString()} Does Not Exist.");
            }

            bool updated = await _doctorUseCase.UpdateDoctorAsync(id, doctorInsertDTO);

            if (!updated) return NotFound("Doutor não encontrado.");

            var infosDoctorUpdate = await _doctorUseCase.GetDoctorByIdAsync(id);
            return Ok(infosDoctorUpdate);

        }
    }
}