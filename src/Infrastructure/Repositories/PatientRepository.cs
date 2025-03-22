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

        public async Task<(int total, IEnumerable<PatientListDTO> patient)> GetPatientsAsync(string? name, string? documentNumber, int? statusId, int itemsPerPage, int page)
        {
            var queryBase = new StringBuilder(@"     
                    FROM PATIENT P
                    INNER JOIN STATUS S ON S.ID = P.STATUSID
                    WHERE 1 = 1");

            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(name))
            {
                queryBase.Append(" AND P.NAME LIKE @Name");
                parameters.Add("Name", $"%{name}%");
            }

            if (!string.IsNullOrEmpty(documentNumber))
            {
                queryBase.Append(" AND P.DOCUMENTNUMBER LIKE @DocumentNumber");
                parameters.Add("DocumentNumber", $"%{documentNumber}%");
            }

            if (statusId.HasValue)
            {
                queryBase.Append(" AND S.ID = @StatusId");
                parameters.Add("StatusId", statusId.Value);
            }

            var countQuery = $"SELECT COUNT(DISTINCT P.ID) {queryBase}";
            int total = await _connection.ExecuteScalarAsync<int>(countQuery, parameters);

            var dataQuery = $@"
                    SELECT 
                        P.ID, 
                        P.NAME,
                        P.PHONENUMBER,
                        P.DOCUMENTNUMBER,
                        P.BIRTHDATE ,
                        P.STATUSID AS STATUSID, 
                        S.NAME AS STATUSNAME
                    {queryBase}
                    ORDER BY P.ID
                    LIMIT @Limit OFFSET @Offset";

            parameters.Add("Limit", itemsPerPage);
            parameters.Add("Offset", (page - 1) * itemsPerPage);

            var patients = await _connection.QueryAsync<PatientListDTO>(dataQuery, parameters);

            return (total, patients);
        }
    }
}