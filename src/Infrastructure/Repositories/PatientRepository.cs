using ClinAgenda.Application.DTOs.Patient;
using Dapper;
using MySql.Data.MySqlClient;

namespace ClinAgenda.Infrastructure.Repositories
{
    public class PatientRepository
    {
        private readonly MySqlConnection _connection;

        public PatientRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<PatientDTO> GetByIdAsync(int id)
        {
            const String query = @"
                SELECT ID, NAME, PHONENUMBER, DOCUMENTNUMBER, STATUSID, BIRTHDATE 
                FROM PATIENT 
                WHERE ID = @ID";
            
            var patient = await _connection.QueryFirstOrDefaultAsync<PatientDTO>(query, new {id = id});

            return patient ?? throw new InvalidOperationException("Patient Not Found.");
        }
    }
}