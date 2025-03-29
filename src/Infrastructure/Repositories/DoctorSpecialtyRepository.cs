using ClinAgenda.Application.DTOs.Doctor;
using ClinAgenda.Core.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;

namespace ClinAgenda.Infrastructure.Repositories
{
    public class DoctorSpecialtyRepository : IDoctorSpecialtyRepository
    {
        private readonly MySqlConnection _connection;

        public DoctorSpecialtyRepository(MySqlConnection mySqlConnection)
        {
            _connection = mySqlConnection;
        }

        public async Task InsertDoctorSpecialtyAsync(DoctorSpecialtyDTO doctorSpecialtyDTO) 
        {
            String query = @"
                INSERT INTO DOCTOR_SPECIALTY (DoctorId, SpecialtyId)
                VALUES (@DoctorId, @SpecialtyId);";

            var parameters = doctorSpecialtyDTO.SpecialtyId.Select(specialtyId => new 
                {
                    DoctorId = doctorSpecialtyDTO.DoctorId,
                    SpecialtyId = specialtyId
                });

            await _connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteDoctorSpecialtyAsync(int doctorId)
        {
            String query = "DELETE FROM doctor_specialty WHERE DoctorId = @DoctorId";
            await _connection.ExecuteAsync(query, new { DoctorId = doctorId });
        }
    }
}