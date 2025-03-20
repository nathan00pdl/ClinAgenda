using ClinAgenda.Application.DTOs;
using Dapper;
using MySql.Data.MySqlClient;

namespace ClinAgenda.Infrastructure.Repositories
{
    public class SpecialtyRepository
    {
        private readonly MySqlConnection _connection;

        public SpecialtyRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<SpecialtyDTO> GetByIdAsync(int id)
        {
            string query = @"
            SELECT ID, 
                   NAME 
            FROM STATUS
            WHERE ID = @Id";

            var parameters = new { Id = id };

            var status = await _connection.QueryFirstOrDefaultAsync<SpecialtyDTO>(query, parameters);

            return status;
        }
    }
}