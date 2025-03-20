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
    }
}