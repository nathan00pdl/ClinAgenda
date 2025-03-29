using ClinAgenda.Application.DTOs;
using ClinAgenda.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ClinAgenda.WebAPI.Controllers
{
    [ApiController]
    [Route("api/appointment")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentUseCase _appointmentUseCase;
        private readonly PatientUseCase _patientUseCase;
        private readonly DoctorUseCase _doctorUseCase;
        private readonly SpecialtyUseCase _specialtyUseCase;

        public AppointmentController (AppointmentUseCase appointmentUseCase, PatientUseCase patientUseCase, DoctorUseCase doctorUseCase, SpecialtyUseCase specialtyUseCase)
        {
            _appointmentUseCase = appointmentUseCase;
            _patientUseCase = patientUseCase;
            _doctorUseCase = doctorUseCase;
            _specialtyUseCase = specialtyUseCase;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GettAllAppointment([FromQuery] String? patientName, [FromQuery] String? doctorName, [FromQuery] int? specialtyId, [FromQuery] int itemsPerPage=10, [FromQuery] int page=1)
        {
            try
            {
                var result = await _appointmentUseCase.GetAllAppointmentAync(patientName, doctorName, specialtyId, itemsPerPage, page);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("listById/{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            try
            {
                var appointment = await _appointmentUseCase.GetAppointmentByIdAsync(id);
                if (appointment == null) return NotFound();
                
                return Ok(appointment); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpPost("insert")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentDTO appointmentDTO)
        {
            try
            {
                var hasDoctor = await _doctorUseCase.GetDoctorByIdAsync(appointmentDTO.DoctorId);
                if (hasDoctor == null)
                {
                    return BadRequest($"The Doctor with ID {appointmentDTO.DoctorId} Does Not Exist.");
                }

                var hasPatient = await _patientUseCase.GetPatientByIdAsync(appointmentDTO.PatientId);
                if (hasPatient == null)
                {
                    return BadRequest($"The Patient with ID {appointmentDTO.PatientId} Does Not Exist.");
                }

                var specialties = await _specialtyUseCase.GetSpecialtyByIdAsync(appointmentDTO.SpecialtyId);
                if (specialties == null) 
                {
                    return BadRequest($"The Specialty with ID {appointmentDTO.SpecialtyId} Does Not Exist.");
                }

                var createdAppointment = await _appointmentUseCase.CreateAppointmentAsync(appointmentDTO);
                var infosAppointmentCreated = await _appointmentUseCase.GetAppointmentByIdAsync(createdAppointment);

                return Ok(infosAppointmentCreated);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] AppointmentDTO appointmentDTO)
        {
            try
            {
                if (appointmentDTO == null) return BadRequest();
                
                var hasDoctor = await _doctorUseCase.GetDoctorByIdAsync(appointmentDTO.DoctorId);
                if (hasDoctor == null)
                {
                    return BadRequest($"The Doctor with ID {appointmentDTO.DoctorId} Does Not Exist.");
                }

                var hasPatient = await _patientUseCase.GetPatientByIdAsync(appointmentDTO.PatientId);
                if (hasPatient == null)
                {
                    return BadRequest($"The Patient with ID {appointmentDTO.PatientId} Does Not Exist.");
                }

                var specialties = await _specialtyUseCase.GetSpecialtyByIdAsync(appointmentDTO.SpecialtyId);
                if (specialties == null) 
                {
                    return BadRequest($"The Specialty with ID {appointmentDTO.SpecialtyId} Does Not Exist.");
                }

                bool updatedAppointment = await _appointmentUseCase.UpdateAppointmentAsync(id, appointmentDTO);
                if (!updatedAppointment) return NotFound("Patient Not Found");

                var infosDoctorUpdate = await _appointmentUseCase.GetAppointmentByIdAsync(id);
                return Ok(infosDoctorUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                var success = await _appointmentUseCase.DeleteAppointmentAsync(id);
                if (!success) 
                {
                    return NotFound($"Appointment with ID {id} Not Found");
                }
                
                return Ok("Appointment Deleted Successfully"); 
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }
    }
}