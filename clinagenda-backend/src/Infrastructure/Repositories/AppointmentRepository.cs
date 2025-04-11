using System.Text;
using ClinAgenda.Application.DTOs;
using ClinAgenda.Application.DTOs.Appointment;
using ClinAgenda.Core.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;

namespace ClinAgenda.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {

        private readonly String _connectionString;

        public AppointmentRepository(String connection)
        {
            _connectionString = connection;
        }

        public async Task<(int total, IEnumerable<AppointmentListDTO> appointment)> GetAllAppointmentAync(String? patientName, String? doctorName, int? specialtyId, int itemsPerPage, int page)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var queryBase = new StringBuilder(@"
                FROM Appointment A 
                INNER JOIN PATIENT P ON P.ID = A.PATIENTID
                INNER JOIN DOCTOR D ON D.ID = A.DOCTORID
                INNER JOIN SPECIALTY S ON S.ID = A.SPECIALTYID
            ");

            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(patientName))
            {
                queryBase.Append(" AND P.NAME LIKE @Name");
                parameters.Add("Name", $"%{patientName}%");
            }

            if (!string.IsNullOrEmpty(doctorName))
            {
                queryBase.Append(" AND D.NAME LIKE @DoctorName");
                parameters.Add("DoctorName", $"%{doctorName}%");
            }

            if (specialtyId.HasValue)
            {
                queryBase.Append(" AND S.ID = @SpecialtyId");
                parameters.Add("SpecialtyId", specialtyId.Value);
            }

            var countQuery = $"SELECT COUNT(DISTINCT A.ID) {queryBase}";
            int total = await connection.ExecuteScalarAsync<int>(countQuery, parameters);

            var dataQuery = $@"
                SELECT 
                    A.ID,
                    P.NAME AS PATIENTNAME,
                    P.DOCUMENTNUMBER AS PATIENTDOCUMENT,
                    D.NAME AS DOCTORNAME,
                    S.ID AS SPECIALTYID,
                    S.NAME AS SPECIALTYNAME,
                    S.SCHEDULEDURATION AS SCHEDULEDURATION,
                    DATE_FORMAT(A.APPOINTMENTDATE, '%d/%m/%Y') AS APPOINTMENTDATE,
                    A.OBSERVATION AS OBSERVATION
                {queryBase}
                ORDER BY A.ID
                LIMIT @Limit OFFSET @Offset";

            parameters.Add("Limit", itemsPerPage);
            parameters.Add("Offset", (page - 1) * itemsPerPage);

            var appointment = await connection.QueryAsync<AppointmentListDTO>(dataQuery, parameters);

            return (total, appointment);
        }

        public async Task<AppointmentDTO?> GetAppointmentByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            String query = "SELECT *, DATE_FORMAT(A.APPOINTMENTDATE, '%d/%m/%Y') AS APPOINTMENTDATE FROM Appointment WHERE Id = @Id;";
            var parameters = new { Id = id };

            return await connection.QueryFirstOrDefaultAsync<AppointmentDTO>(query, parameters);
        }

        public async Task<int> InsertAppointmentAsync(AppointmentDTO appointmentDTO)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            String query = @"
                INSERT INTO Appointment (patientId, doctorId, specialtyId, appointmentDate, observation)
                VALUES (@patientId, @doctorId, @specialtyId, @appointmentDate, @observation);

                SELECT LAST_INSERT_ID();";

            return await connection.ExecuteScalarAsync<int>(query, appointmentDTO);
        }

        public async Task<bool> UpdateAppointmentAsync(AppointmentInsertDTO appointmentInsertDTO)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            String query = @"
                UPDATE Appointment SET 
                    patientId = @PatientId,
                    doctorId = @DoctorId,
                    specialtyId = @SpecialtyId,
                    appointmentDate = @AppointmentDate,
                    observation = @Observation
                WHERE Id = @Id;";

            int rowsAffected = await connection.ExecuteAsync(query, appointmentInsertDTO);
            return rowsAffected > 0;
        }

        public async Task<int> DeleteAppointmentAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            String query = "DELETE FROM Appointment WHERE ID = @Id";
            var parameters = new { Id = id };

            return await connection.ExecuteAsync(query, parameters);
        }
    }
}