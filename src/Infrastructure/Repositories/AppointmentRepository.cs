using System.Text;
using ClinAgenda.Application.DTOs;
using ClinAgenda.Application.DTOs.Appointment;
using ClinAgenda.Application.DTOs.Patient;
using ClinAgenda.Core.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1;

namespace ClinAgenda.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {

        private readonly MySqlConnection _connection;

        public AppointmentRepository(MySqlConnection mySqlConnection)
        {
            _connection = mySqlConnection;
        }

        public async Task<(int total, IEnumerable<AppointmentListDTO> appointment)> GetAllAppointmentAync(string? patientName, string? doctorName, int? specialtyId, int itemsPerPage, int page)
        {
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

            var counQuery = $"SELECT COUNT(DISTINCT A.ID) {queryBase}";
            
            int total = await _connection.ExecuteScalarAsync<int>(counQuery, parameters);

            var dataQuery = $@"
                SELECT 
                    A.ID,
                    P.NAME AS PATIENTNAME,
                    P.DOCUMENTNUMBER AS PATIENTDOCUMENT,
                    D.NAME AS DOCTORNAME,
                    S.ID AS SPECIALTYID,
                    S.NAME AS SPECIALTYNAME,
                    S.SCHEDULEDURATION AS SCHEDULEDURATION,
                    A.APPOINTMENTDATE AS APPOINTMENTDATE
                {queryBase}
                ORDER BY A.ID
                LIMIT @Limit OFFSET @Offset";

            parameters.Add("Limit", itemsPerPage);
            parameters.Add("Offset", (page - 1) * itemsPerPage);

            var appointment = await _connection.QueryAsync<AppointmentListDTO>(dataQuery, parameters);
            
            return (total, appointment);
        }

        public async Task<AppointmentDTO?> GetAppointmentByIdAsync(int id)
        {
            string query = "SELECT * FROM Appointment WHERE Id = @Id;";
            return await _connection.QueryFirstOrDefaultAsync<AppointmentDTO>(query, new { Id = id });
        }

        public async Task<int> InsertAppointmentAsync(AppointmentDTO appointmentDTO)
        {
            string query = @"
            INSERT INTO Appointment (patientId, doctorId, specialtyId, appointmentDate, observation)
            VALUES (@patientId, @doctorId, @specialtyId, @appointmentDate, @observation);
            SELECT LAST_INSERT_ID();";

            return await _connection.ExecuteScalarAsync<int>(query, appointmentDTO);
        }

        public async Task<bool> UpdateAppointmentAsync(AppointmentInsertDTO appointmentInsertDTO)
        {
            string query = @"
                UPDATE Appointment SET 
                    patientId = @PatientId,
                    doctorId = @DoctorId,
                    specialtyId = @SpecialtyId,
                    appointmentDate = @AppointmentDate,
                    observation = @Observation
                WHERE Id = @Id;";
            
            int rowsAffected = await _connection.ExecuteAsync(query, appointmentInsertDTO);
            return rowsAffected > 0;
        }

        public async Task<int> DeleteAppointmentAsync(int id)
        {
            string query = "DELETE FROM Appointment WHERE ID = @AppointmentId";
            
            return await _connection.ExecuteAsync(query, new { AppointmentId = id });
        }
    }
}