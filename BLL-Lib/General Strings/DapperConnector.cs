using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.General_Strings
{
    public static class DapperConnector
    {
        public static string Using() =>
            @"using Dapper;
            using Microsoft.Extensions.Configuration;
            using System;
            using System.Collections.Generic;
            using System.Data;
            using System.Data.SqlClient;
            using System.Threading.Tasks;";

        public static string FileName() =>
            @"SqlDataAccess";

        public static string Interface() =>
            @"string ConnectionStringName { get; set; }

            Task<int> ExecuteAsync<T>(string sql, T parameters, CommandType commandType = CommandType.StoredProcedure);
            Task<T> GetFirstAsync<T, U>(string sql, U parameters, CommandType commandType);
            Task<IEnumerable<T>> GetListAsync<T, U>(string sql, U parameters, CommandType commandType);
            IDbConnection SqlConnection();";

        public static string Properties() =>
            @"public string ConnectionStringName { get; set; } = ""Default"";";

        public static string Constructor() =>
            @"private readonly IConfiguration _config;

            public SqlDataAccess(IConfiguration config)
            {
                _config = config;
            }";

        public static string Connector() =>
            @"public IDbConnection SqlConnection()
            {
                string connectionString = _config.GetConnectionString(ConnectionStringName);
                return new SqlConnection(connectionString);
            }";

        public static string GetListAsync() =>
            @"public async Task<IEnumerable<T>> GetListAsync<T, U>(string sql, U parameters, CommandType commandType)
            {
                using IDbConnection connection = SqlConnection();
                try
                {
                    return await connection.QueryAsync<T>(sql, parameters, commandType: commandType);
                }
                catch
                {
                    throw;
                }
            }";

        public static string GetFirstAsync() =>
            @"public async Task<T> GetFirstAsync<T, U>(string sql, U parameters, CommandType commandType)
            {
                using IDbConnection connection = SqlConnection();
                try
                {
                    return await connection.QueryFirstAsync<T>(sql, parameters, commandType: commandType);
                }
                catch
                {
                    throw;
                }
            }";

        public static string ExecuteAsync() =>
            @"public async Task<int> ExecuteAsync<T>(string sql, T parameters, CommandType commandType = CommandType.StoredProcedure)
            {
                using IDbConnection connection = SqlConnection();
                try
                {
                    return await connection.ExecuteAsync(sql, parameters, commandType: commandType);
                }
                catch
                {
                    throw;
                }
            }";
    }
}
