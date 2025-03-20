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
            const string query = @"
                SELECT 
                    ID, 
                    NAME,
                    SCHEDULEDURATION 
                FROM SPECIALTY
                WHERE ID = @Id";

            var specialty = await _connection.QueryFirstOrDefaultAsync<SpecialtyDTO>(query, new { Id = id });

            return specialty;
        }
    }
}