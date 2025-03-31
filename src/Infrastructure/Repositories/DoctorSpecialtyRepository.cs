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

        public async Task<int> InsertDoctorSpecialtyAsync(DoctorSpecialtyDTO doctorSpecialtyDTO) 
        {
            String query = @"
                INSERT INTO DOCTOR_SPECIALTY (DoctorId, SpecialtyId)
                VALUES (@DoctorId, @SpecialtyId);

                SELECT LAST_INSERT_ID();
            ";

            var parameters = doctorSpecialtyDTO.SpecialtyId.Select(specialtyId => new 
                {
                    DoctorId = doctorSpecialtyDTO.DoctorId,
                    SpecialtyId = specialtyId
                });

            return await _connection.ExecuteAsync(query, parameters);
        }

        public async Task<int> DeleteDoctorSpecialtyAsync(int id)
        {
            String query = "DELETE FROM doctor_specialty WHERE DoctorId = @Id";
            var parameters = new { Id = id };

            return await _connection.ExecuteAsync(query, parameters);
        }
    }
}