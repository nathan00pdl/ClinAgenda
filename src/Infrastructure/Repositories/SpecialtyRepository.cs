using System.Text;
using ClinAgenda.Application.DTOs;
using ClinAgenda.Core.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;

namespace ClinAgenda.Infrastructure.Repositories
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        private readonly MySqlConnection _connection;

        public SpecialtyRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<(int total, IEnumerable<SpecialtyDTO> specialtys)> GetAllSpecialtyAsync(int? itemsPerPage, int? page)
        {
            var queryBase = new StringBuilder(@"FROM SPECIALTY S WHERE 1 = 1 ");
            var parameters = new DynamicParameters();
            var countQuery = $"SELECT COUNT(DISTINCT S.ID) {queryBase}";
            
            int total = await _connection.ExecuteScalarAsync<int>(countQuery, parameters);

            var dataQuery = $@"
                SELECT 
                    ID, 
                    NAME, 
                    SCHEDULEDURATION 
                {queryBase}
                LIMIT @Limit OFFSET @Offset";

            parameters.Add("Limit", itemsPerPage);
            parameters.Add("Offset", (page - 1) * itemsPerPage);

            var specialtys = await _connection.QueryAsync<SpecialtyDTO>(dataQuery, parameters);

            return (total, specialtys);
        }

        public async Task<SpecialtyDTO> GetSpecialtyByIdAsync(int id)
        {
            const String query = @"
                SELECT 
                    ID, 
                    NAME, 
                    SCHEDULEDURATION 
                FROM SPECIALTY
                WHERE ID = @Id";

            var parameters = new { Id = id };

            return await _connection.QueryFirstOrDefaultAsync<SpecialtyDTO>(query, parameters) ?? throw new KeyNotFoundException($"Specialty with ID {id} was not found.");;
        }

        public async Task<IEnumerable<SpecialtyDTO>> GetSpecialtiesByIds(List<int> id)
        {
            var query = @"
                SELECT 
                    S.ID, 
                    S.NAME,
                    S.SCHEDULEDURATION 
                FROM SPECIALTY S
                WHERE S.ID IN @ID";

            var parameters = new { Id = id };

            return await _connection.QueryAsync<SpecialtyDTO>(query, parameters);
        }

        public async Task<int> InsertSpecialtyAsync(SpecialtyInsertDTO specialtyInsertDTO)
        {
            String query = @"
                INSERT INTO SPECIALTY (NAME, SCHEDULEDURATION) 
                VALUES (@Name, @Scheduleduration);

                SELECT LAST_INSERT_ID();
            ";
                
            return await _connection.ExecuteScalarAsync<int>(query, specialtyInsertDTO);
        }

        public async Task<int> DeleteSpecialtyAsync(int id)
        {
            String query = @"DELETE FROM SPECIALTY WHERE ID = @Id";
            var parameters = new { Id = id };

            return await _connection.ExecuteAsync(query, parameters);
        }
    }
}