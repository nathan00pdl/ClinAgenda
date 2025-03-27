using System.Text;
using ClinAgenda.Application.DTOs.Doctor;
using ClinAgenda.Core.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;

namespace ClinAgenda.Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {

        private readonly MySqlConnection _connection;

        public DoctorRepository(MySqlConnection mySqlConnection)
        {
            _connection = mySqlConnection;
        }

        public async Task<IEnumerable<DoctorListDTO>> GetDoctorsAsync(string? name, int? specialtyId, int? statusId, int offset, int itemsPerPage)
        {
            var innerJoins = new StringBuilder(@"
                FROM DOCTOR D
                INNER JOIN STATUS S ON STATUSID = S.ID
                INNER JOIN DOCTOR_SPECIALTY DSPE ON DSPE.DOCTORID = D.ID
                WHERE 1 = 1 "
            );

            var parameters = new DynamicParameters();

            if (!String.IsNullOrEmpty(name))
            {
                innerJoins.Append("AND D.NAME LIKE @Name");
                parameters.Add("Name", $"{name}");
            }

            if (specialtyId.HasValue)
            {
                innerJoins.Append("AND DSPE.SPECIALTYID = @SpecialtyId");
                parameters.Add("SpecialtyId", specialtyId.Value);
            }

            if (statusId.HasValue)
            {
                innerJoins.Append("AND S.ID = @StatusId");
                parameters.Add("StatusId", statusId.Value);
            }

            parameters.Add("LIMIT", itemsPerPage);
            parameters.Add("OFFSET", offset);

            var query = $@"
                SELECT DISTINCT 
                    D.ID AS ID,
                    S.NAME AS NAME,
                    S.ID AS STATUSID,
                    S.NAME AS STATUSNAME
                {innerJoins}
                ORDER BY D.ID
                LIMIT @Limit OFFSET @Offset";
            
            return await _connection.QueryAsync<DoctorListDTO>(query.ToString(), parameters);
        }

        public Task<IEnumerable<DoctorListDTO>> GetDoctorsByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SpecialtyDoctorDTO>> GetDoctorSpecialtiesAsync(int[] doctorIds)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertDoctorAsync(DoctorInsertDTO doctorInsertDTO)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDoctorAsync(DoctorDTO doctorDTO)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteDoctorAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}