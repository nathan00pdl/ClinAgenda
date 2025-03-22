using System.Text;
using ClinAgenda.Application.DTOs;
using MySql.Data.MySqlClient;
using Dapper;
using ClinAgenda.Core.Entities;
using ClinAgenda.Core.Interfaces;

namespace ClinAgenda.Infrastructure.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly MySqlConnection _connection;

        public StatusRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<StatusDTO> GetByIdAsync(int id)
        {
            String query = @"
                SELECT 
                    ID, 
                    NAME 
                FROM STATUS
                WHERE ID = @Id";

            var parameters = new { Id = id };

            var status = await _connection.QueryFirstOrDefaultAsync<StatusDTO>(query, parameters);

            return status ?? throw new InvalidOperationException("Status Not Found.");
        }

        public async Task<int> InsertStatusAsync(StatusInsertDTO statusInsertDTO)
        {
            String query = @"
                INSERT INTO STATUS (NAME) 
                VALUES (@Name);
                SELECT LAST_INSERT_ID();";

            return await _connection.ExecuteScalarAsync<int>(query, statusInsertDTO);
        }

        public async Task<int> DeleteStatusAsync(int id)
        {
            String query = @"
                DELETE FROM STATUS
                WHERE ID = @Id";

            var parameters = new { Id = id };

            var rowsAffected = await _connection.ExecuteAsync(query, parameters);

            return rowsAffected;
        }

        public async Task<(int total, IEnumerable<StatusDTO> specialtys)> GetAllAsync(int? itemsPerPage, int? page)
        {
            var queryBase = new StringBuilder(@"FROM STATUS S WHERE 1 = 1"); // "1 = 1" is used to facilitate the addition of dynamic filters

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
    }
}