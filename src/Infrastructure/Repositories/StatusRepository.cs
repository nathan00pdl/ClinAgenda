using ClinAgenda.Application.DTOs;
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
            string query = @"
            SELECT ID, 
                   NAME 
            FROM STATUS
            WHERE ID = @Id";

            var parameters = new { Id = id };

            var status = await _connection.QueryFirstOrDefaultAsync<StatusDTO>(query, parameters);

            return status;
        }

        public async Task<int> DeleteStatusAsync(int id)
        {
            string query = @"
            DELETE FROM STATUS
            WHERE ID = @Id";

            var parameters = new { Id = id };

            var rowsAffected = await _connection.ExecuteAsync(query, parameters);

            return rowsAffected;
        }
        
        public async Task<int> InsertStatusAsync(StatusInsertDTO statusInsertDTO)
        {
            string query = @"
            INSERT INTO STATUS (NAME) 
            VALUES (@Name);
            SELECT LAST_INSERT_ID();"; 

            return await _connection.ExecuteScalarAsync<int>(query, statusInsertDTO);
        }
    }
}