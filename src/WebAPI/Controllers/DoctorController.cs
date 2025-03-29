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
        private readonly AppointmentUseCase _appointmentUseCase;

        public DoctorController(DoctorUseCase doctorUseCase, StatusUseCase statusUseCase, SpecialtyUseCase specialtyUseCase, AppointmentUseCase appointmentService)
        {
            _doctorUseCase = doctorUseCase;
            _statusUseCase = statusUseCase;
            _specialtyUseCase = specialtyUseCase;
            _appointmentUseCase = appointmentService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllDoctor([FromQuery] string? name, [FromQuery] int? specialtyId, [FromQuery] int? statusId, [FromQuery] int itemsPerPage = 10, [FromQuery] int page = 1)
        {
            try
            {
                var result = await _doctorUseCase.GetAllDoctorAsync(name, specialtyId, statusId, itemsPerPage, page);
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
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorInsertDTO doctorInsertDTO)
        {
            if (doctorInsertDTO == null) return BadRequest();

            var hasStatus = await _statusUseCase.GetStatusByIdAsync(doctorInsertDTO.StatusId);
            if (hasStatus == null)
                return BadRequest($"The Status with ID {doctorInsertDTO.StatusId} Does Not Exist.");

            var specialties = await _specialtyUseCase.GetSpecialtiesByIds(doctorInsertDTO.Specialty);

            var notFoundSpecialties = doctorInsertDTO.Specialty.Except(specialties.Select(s => s.Id)).ToList();

            if (notFoundSpecialties.Any())
            {
                return BadRequest(notFoundSpecialties.Count > 1 ? $"The Specialties with IDs {string.Join(", ", notFoundSpecialties)} Does Not Exist" : $"The Specialty with ID {notFoundSpecialties.First().ToString()} Does Not Exist.");
            }

            bool updated = await _doctorUseCase.UpdateDoctorAsync(id, doctorInsertDTO);

            if (!updated) return NotFound("Doctor Not Found.");

            var infosDoctorUpdate = await _doctorUseCase.GetDoctorByIdAsync(id);
            return Ok(infosDoctorUpdate);

        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            try
            {
                var doctorInfo = await _doctorUseCase.GetDoctorByIdAsync(id);
                var appointment = await _appointmentUseCase.GetAllAppointmentAync(null, doctorName: doctorInfo.Name, null, 1, 1);
                
                if (appointment.Total > 0) 
                {
                    return NotFound($"Erro Deleting, Doctor with Appointment!");
                }
                
                var success = await _doctorUseCase.DeleteDoctorASync(id);
                if (!success)
                {
                    return NotFound($"Doctor with ID {id} Not Found!");
                }

                return Ok("Doctor Deleted Successfully"); 
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}