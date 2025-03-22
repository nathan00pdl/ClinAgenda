using System.Text;
using ClinAgenda.Application.DTOs.Patient;
using Dapper;
using MySql.Data.MySqlClient;

namespace ClinAgenda.Infrastructure.Repositories
{
    public class PatientRepository
    {
        private readonly MySqlConnection _connection;

        public PatientRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<PatientDTO> GetByIdAsync(int id)
        {
            const String query = @"
                SELECT 
                    ID, 
                    NAME, 
                    PHONENUMBER, 
                    DOCUMENTNUMBER, 
                    STATUSID, 
                    BIRTHDATE 
                FROM PATIENT 
                WHERE ID = @ID";

            var patient = await _connection.QueryFirstOrDefaultAsync<PatientDTO>(query, new { id = id });

            return patient ?? throw new InvalidOperationException("Patient Not Found.");
        }

        public async Task<int> InsertPatientAsync(PatientInsertDTO patient)
        {
            string query = @"
            INSERT INTO Patient (name, phoneNumber, documentNumber, statusId, birthDate) 
            VALUES (@Name, @PhoneNumber, @DocumentNumber, @StatusId, @BirthDate);
            SELECT LAST_INSERT_ID();";
            return await _connection.ExecuteScalarAsync<int>(query, patient);
        }

        public async Task<bool> UpdatePatientAsync(PatientDTO patient)
        {
            string query = @"
            UPDATE Patient SET 
                Name = @Name,
                phoneNumber = @PhoneNumber,
                documentNumber = @DocumentNumber,
                birthDate = @BirthDate,
                StatusId = @StatusId
            WHERE Id = @Id;";
            int rowsAffected = await _connection.ExecuteAsync(query, patient);
            return rowsAffected > 0;
        }

        public async Task<int> DeleteByPatientIdAsync(int id)
        {
            string query = "DELETE FROM Patient WHERE ID = @Id";

            var parameters = new { Id = id };

            var rowsAffected = await _connection.ExecuteAsync(query, parameters);

            return rowsAffected;
        }
    }
}