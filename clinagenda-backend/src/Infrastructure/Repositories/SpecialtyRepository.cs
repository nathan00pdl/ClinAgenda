using System.Text;
using ClinAgenda.Application.DTOs;
using ClinAgenda.Core.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;

namespace ClinAgenda.Infrastructure.Repositories
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        private readonly String _connectionString;

        public SpecialtyRepository(String connection)
        {
            _connectionString = connection;
        }

        public async Task<(int total, IEnumerable<SpecialtyDTO> specialtys)> GetAllSpecialtyAsync(int? itemsPerPage, int? page)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var queryBase = new StringBuilder(@"FROM SPECIALTY S WHERE 1 = 1 ");
            var parameters = new DynamicParameters();
            var countQuery = $"SELECT COUNT(DISTINCT S.ID) {queryBase}";

            int total = await connection.ExecuteScalarAsync<int>(countQuery, parameters);

            var dataQuery = $@"
                SELECT 
                    ID, 
                    NAME, 
                    SCHEDULEDURATION 
                {queryBase}
                LIMIT @Limit OFFSET @Offset";

            parameters.Add("Limit", itemsPerPage);
            parameters.Add("Offset", (page - 1) * itemsPerPage);

            var specialtys = await connection.QueryAsync<SpecialtyDTO>(dataQuery, parameters);

            return (total, specialtys);
        }

        public async Task<SpecialtyDTO> GetSpecialtyByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            const String query = @"
                SELECT 
                    ID, 
                    NAME, 
                    SCHEDULEDURATION 
                FROM SPECIALTY
                WHERE ID = @Id";

            var parameters = new { Id = id };

            return await connection.QueryFirstOrDefaultAsync<SpecialtyDTO>(query, parameters) ?? throw new KeyNotFoundException($"Specialty with ID {id} Not Found."); ;
        }

        public async Task<IEnumerable<SpecialtyDTO>> GetSpecialtiesByIds(List<int> id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT 
                    S.ID, 
                    S.NAME,
                    S.SCHEDULEDURATION 
                FROM SPECIALTY S
                WHERE S.ID IN @ID";

            var parameters = new { Id = id };

            return await connection.QueryAsync<SpecialtyDTO>(query, parameters);
        }

        public async Task<int> InsertSpecialtyAsync(SpecialtyInsertDTO specialtyInsertDTO)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            String query = @"
                INSERT INTO SPECIALTY (NAME, SCHEDULEDURATION) 
                VALUES (@Name, @Scheduleduration);

                SELECT LAST_INSERT_ID();
            ";

            return await connection.ExecuteScalarAsync<int>(query, specialtyInsertDTO);
        }

        public async Task<bool> UpdateSpecialtyAsync(SpecialtyDTO specialtyDTO)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            String query = @"UPDATE Specialty SET Name = @Name, ScheduleDuration = @ScheduleDuration WHERE Id = @Id;";

            int rowsAffected = await connection.ExecuteAsync(query, specialtyDTO);
            return rowsAffected > 0;
        }

        public async Task<int> DeleteSpecialtyAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            String query = @"DELETE FROM SPECIALTY WHERE ID = @Id";
            var parameters = new { Id = id };

            return await connection.ExecuteAsync(query, parameters);
        }
    }
}