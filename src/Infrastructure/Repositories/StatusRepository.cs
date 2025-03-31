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
        private readonly MySqlConnection _connection;

        // Injection Dependency by constructor
        public StatusRepository(MySqlConnection connection)
        {
            _connection = connection;
        }
        
        public async Task<(int total, IEnumerable<StatusDTO> specialtys)> GetAllStatusAsync(int? itemsPerPage, int? page)
        {
            var queryBase = new StringBuilder(@"
                FROM STATUS S WHERE 1 = 1" // "1 = 1" is used to facilitate the addition of dynamic filters
                ); 

            var parameters = new DynamicParameters(); 
            
            var countQuery = $"SELECT COUNT(DISTINCT S.ID) {queryBase}";

            int total = await _connection.ExecuteScalarAsync<int>(countQuery, parameters);

            var dataQuery = $@"
                SELECT 
                    ID, 
                    NAME
                {queryBase}
                LIMIT @Limit OFFSET @Offset";

            parameters.Add("Limit", itemsPerPage);
            parameters.Add("Offset", (page - 1) * itemsPerPage);

            var status = await _connection.QueryAsync<StatusDTO>(dataQuery, parameters);

            return (total, status); 
        }

        public async Task<StatusDTO> GetStatusByIdAsync(int id)
        {
            String query = @"SELECT ID, NAME FROM STATUS WHERE ID = @Id";
            var parameters = new { Id = id }; // Creating an anonymous object to avoid SQL injection

            return await _connection.QueryFirstOrDefaultAsync<StatusDTO>(query, parameters);
        }

        public async Task<int> InsertStatusAsync(StatusInsertDTO statusInsertDTO)
        {
            String query = @"
                INSERT INTO STATUS (NAME) 
                VALUES (@Name);
                
                SELECT LAST_INSERT_ID();";

            return await _connection.ExecuteScalarAsync<int>(query, statusInsertDTO); // @Name is passed by statusInsertDTO
        }

        public async Task<int> DeleteStatusAsync(int id)
        {
            String query = @"DELETE FROM STATUS WHERE ID = @Id";
            var parameters = new { Id = id };

            return await _connection.ExecuteAsync(query, parameters);
        }

    }
}