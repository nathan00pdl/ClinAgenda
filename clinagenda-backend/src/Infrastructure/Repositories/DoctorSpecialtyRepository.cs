using ClinAgenda.Application.DTOs.Doctor;
using ClinAgenda.Core.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;

namespace ClinAgenda.Infrastructure.Repositories
{
    public class DoctorSpecialtyRepository : IDoctorSpecialtyRepository
    {
        private readonly String _connectionString;

        public DoctorSpecialtyRepository(String connection)
        {
            _connectionString = connection;
        }

        public async Task<int> InsertDoctorSpecialtyAsync(DoctorSpecialtyDTO doctorSpecialtyDTO) 
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

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

            return await connection.ExecuteAsync(query, parameters);
        }

        public async Task<int> DeleteDoctorSpecialtyAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            String query = "DELETE FROM doctor_specialty WHERE DoctorId = @Id";
            var parameters = new { Id = id };

            return await connection.ExecuteAsync(query, parameters);
        }
    }
}