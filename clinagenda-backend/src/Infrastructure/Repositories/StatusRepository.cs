using System.Text;
using ClinAgenda.Application.DTOs;
using MySql.Data.MySqlClient;
using Dapper;
using ClinAgenda.Core.Entities;
using ClinAgenda.Core.Interfaces;

/* 
 * Note
 *
 * Injection Dependency by constructor avoid recreating the connection with the database.
 */

namespace ClinAgenda.Infrastructure.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly String _connectionString;

        // Injection Dependency by constructor
        public StatusRepository(String connection)
        {
            _connectionString = connection;
        }
        
        public async Task<(int total, IEnumerable<StatusDTO> specialtys)> GetAllStatusAsync(int? itemsPerPage, int? page)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var queryBase = new StringBuilder(@"
                    FROM STATUS S WHERE 1 = 1 " // "1 = 1" is used to facilitate the addition of dynamic filters
                );

            var parameters = new DynamicParameters();

            var countQuery = $"SELECT COUNT(DISTINCT S.ID) {queryBase}";

            int total = await connection.ExecuteScalarAsync<int>(countQuery, parameters);

            var dataQuery = $@"
                    SELECT 
                        ID, 
                        NAME
                    {queryBase}
                    LIMIT @Limit OFFSET @Offset";

            parameters.Add("Limit", itemsPerPage);
            parameters.Add("Offset", (page - 1) * itemsPerPage);

            var status = await connection.QueryAsync<StatusDTO>(dataQuery, parameters);

            return (total, status);
        }

        public async Task<StatusDTO> GetStatusByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            String query = @"SELECT ID, NAME FROM STATUS WHERE ID = @Id";
            var parameters = new { Id = id }; // Creating an anonymous object to avoid SQL injection

            return await connection.QueryFirstOrDefaultAsync<StatusDTO>(query, parameters) ?? throw new KeyNotFoundException($"Status with ID {id} not found.");
        }

        public async Task<int> InsertStatusAsync(StatusInsertDTO statusInsertDTO)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            String query = @"
                INSERT INTO STATUS (NAME) 
                VALUES (@Name);
                
                SELECT LAST_INSERT_ID();
            ";

            return await connection.ExecuteScalarAsync<int>(query, statusInsertDTO); // @Name is passed by statusInsertDTO
        }

        public async Task<int> DeleteStatusAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            String query = @"DELETE FROM STATUS WHERE ID = @Id";
            var parameters = new { Id = id };

            return await connection.ExecuteAsync(query, parameters);
        }

    }
}