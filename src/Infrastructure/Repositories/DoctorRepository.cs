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

        public async Task<IEnumerable<DoctorListDTO>> GetDoctorsByIdAsync(int id)
        {
            var queryBase = new StringBuilder(@"
                FROM DOCTOR D
                LEFT JOIN DOCTOR_SPECIALTY DSPE ON D.ID = DSPE.DOCTORID
                LEFT JOIN STATUS S ON S.ID = D.STATUSID
                LEFT JOIN SPECIALTY SP ON SP.ID = DSPE.SPECIALTYID
                WHERE 1 = 1 "
            );

            var parameters = new DynamicParameters();

            if (id > 0)
            {
                queryBase.Append(" AND D.ID = @id");
                parameters.Add("id", id);
            }    

            var dataQuery = $@"
                SELECT DISTINCT 
                    D.ID,
                    D.NAME,
                    D.STATUSID AS STATUSID,
                    D.NAME AS STATUSNAME,
                    DSPE.SPECIALTYID AS SPECIALTYID,
                    SP.NAME AS SPECIALTYNAME
                {queryBase}
                ORDER BY D.ID";

                return await _connection.QueryAsync<DoctorListDTO>(dataQuery, parameters);
        }

        public async Task<IEnumerable<SpecialtyDoctorDTO>> GetDoctorSpecialtiesAsync(int[] doctorIds)
        {
            var query = @"
                SELECT 
                    DS.DOCTORID AS DOCTORID,
                    SP.ID AS SPECIALTYID,
                    SP.NAME AS SPECIALTYNAME,
                    SP.SCHEDULEDURATION 
                FROM DOCTOR_SPECIALTY DS
                INNER JOIN SPECIALTY SP ON DS.SPECIALTYID = SP.ID
                WHERE DS.DOCTORID IN @DOCTORIDS";
            
            var parameters = new { DoctorIds = doctorIds };

            return await _connection.QueryAsync<SpecialtyDoctorDTO>(query, parameters);
        }

        public async Task<int> InsertDoctorAsync(DoctorInsertDTO doctorInsertDTO)
        {
            String query = @"
                INSERT INTO Doctor (Name, StatusId)
                VALUES (@Name, @StatusId);
                SELECT LAST_INSERT_ID();";

                return await _connection.ExecuteScalarAsync<int>(query, doctorInsertDTO);
        }

        public async Task<bool> UpdateDoctorAsync(DoctorDTO doctorDTO)
        {
            String query = @"
                UPDATE Doctor SET 
                    Name = @Name, 
                    StatusId = @StatusId
                WHERE Id = @Id";

            var rowsAffected = await _connection.ExecuteAsync(query, doctorDTO);
            return rowsAffected > 0;
        }

        public async Task<int> DeleteDoctorAsync(int id)
        {
            String query = "DELETE FROM DOCTOR WHERE ID = @Id";

            var parameters = new { Id = id };

            var rowsAffected = await _connection.ExecuteAsync(query, parameters);
            return rowsAffected;
        }
    }
}