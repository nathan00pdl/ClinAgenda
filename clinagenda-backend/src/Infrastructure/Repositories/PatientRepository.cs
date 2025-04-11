using System.Text;
using ClinAgenda.Application.DTOs.Patient;
using ClinAgenda.Core.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;

namespace ClinAgenda.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly String _connectionString;

        public PatientRepository(String connection)
        {
            _connectionString = connection;
        }

        public async Task<(int total, IEnumerable<PatientListDTO> patient)> GetAllPatientAsync(String? name, String? documentNumber, int? statusId, int itemsPerPage, int page)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var queryBase = new StringBuilder(@"     
                FROM PATIENT P
                INNER JOIN STATUS S ON S.ID = P.STATUSID
                WHERE 1 = 1 "
            );

            var parameters = new DynamicParameters();

            if (!String.IsNullOrEmpty(name))
            {
                queryBase.Append(" AND P.NAME LIKE @Name");
                parameters.Add("Name", $"%{name}%");
            }

            if (!String.IsNullOrEmpty(documentNumber))
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
            int total = await connection.ExecuteScalarAsync<int>(countQuery, parameters);

            var dataQuery = $@"
                SELECT 
                    P.ID, 
                    P.NAME,
                    P.PHONENUMBER,
                    P.DOCUMENTNUMBER,
                    DATE_FORMAT(BIRTHDATE, '%d/%m/%Y') AS BIRTHDATE,
                    P.STATUSID AS STATUSID, 
                    S.NAME AS STATUSNAME
                {queryBase}
                ORDER BY P.ID
                LIMIT @Limit OFFSET @Offset";

            parameters.Add("Limit", itemsPerPage);
            parameters.Add("Offset", (page - 1) * itemsPerPage);

            var patients = await connection.QueryAsync<PatientListDTO>(dataQuery, parameters);

            return (total, patients);
        }

        public async Task<PatientListDTO?> GetPatientByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            const String query = @"
                SELECT 
                    P.ID, 
                    P.NAME,
                    P.PHONENUMBER,
                    P.DOCUMENTNUMBER,
                    P.STATUSID,
                    DATE_FORMAT(P.BIRTHDATE, '%d/%m/%Y') AS BIRTHDATE,
                    S.NAME AS STATUSNAME
                FROM PATIENT P
                INNER JOIN STATUS S ON S.ID = P.STATUSID
                WHERE P.ID = @Id";

            var parameters = new { Id = id };   

            return await connection.QueryFirstOrDefaultAsync<PatientListDTO>(query, parameters);
        }

        public async Task<int> InsertPatientAsync(PatientInsertDTO patientInsertDTO) 
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            String query = @"
                INSERT INTO Patient (name, phoneNumber, documentNumber, statusId, birthDate) 
                VALUES (@Name, @PhoneNumber, @DocumentNumber, @StatusId, @BirthDate);
                
                SELECT LAST_INSERT_ID();
            ";

            return await connection.ExecuteScalarAsync<int>(query, patientInsertDTO);
        }

        public async Task<bool> UpdatePatientAsync(PatientDTO patientDTO)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            String query = @"
                UPDATE Patient SET 
                    Name = @Name,
                    phoneNumber = @PhoneNumber,
                    documentNumber = @DocumentNumber,
                    birthDate = @BirthDate,
                    StatusId = @StatusId
                WHERE Id = @Id;";

            int rowsAffected = await connection.ExecuteAsync(query, patientDTO);
            return rowsAffected > 0;
        }

        public async Task<int> DeletePatientAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            String query = "DELETE FROM Patient WHERE ID = @Id";
            var parameters = new { Id = id };

            return await connection.ExecuteAsync(query, parameters);
        }

        public async Task<IEnumerable<PatientListDTO>> AutoCompletePatientAsync(String name)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var queryBase = new StringBuilder(@"
                FROM PATIENT P 
                INNER JOIN STATUS S ON S.ID = P.STATUSID
                WHERE 1 = 1 "
            );

            var parameters = new DynamicParameters();

            if (!String.IsNullOrEmpty(name))
            {
                queryBase.Append(" AND P .NAME LIKE @Name");
                parameters.Add("Name", $"{name}");
            }

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
                ORDER BY P.ID";
            
            return await connection.QueryAsync<PatientListDTO>(dataQuery, parameters);
        }

    }
}