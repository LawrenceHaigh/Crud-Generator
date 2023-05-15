using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.General_Strings
{
    public static class Functions
    {
        public static string Create(string tableName, string tableNameSingularized, string tableNameTitleCaseSingularized)
        {
            return $@"public async Task CreateAsync(List<{tableNameSingularized}Request> request)
            {{
                DataTable {tableNameTitleCaseSingularized}Table = request.ToDataTable();

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(""@{tableNameSingularized}"", data.AsTableValuedParameter(""udt_{tableNameSingularized}_Insert""));

                await db.ExecuteAsync(""sp_{tableNameSingularized}_Insert"", dynamicParameters);
            }}";
        }

        public static string Update(string tableName, string tableNameSingularized, string tableNameTitleCaseSingularized)
        {
            return $@"public async Task UpdateAsync(List<{tableNameSingularized}Request> request)
            {{
                DataTable {tableNameTitleCaseSingularized}Table = request.ToDataTable();

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(""@{tableNameSingularized}"", data.AsTableValuedParameter(""udt_{tableNameSingularized}_Update""));

                await db.ExecuteAsync(""sp_{tableNameSingularized}_Update"", dynamicParameters);
            }}";
        }

        public static string Delete(string tableName, string idType)
        {
            return $@"public async Task<HttpStatusCode> DeleteAsync({idType} id)
            {{
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add(""@Id"", id);

                try
                {{
                    await db.ExecuteAsync(""DELETE FROM [{tableName}] WHERE Id = @Id"", dynamicParameters);
                    return HttpStatusCode.OK;
                }}
                catch
                {{
                    throw;
                }}
            }}";
        }

        public static string GetById(string tableName, string tableNameSingularized, string properties, string idType)
        {
            return $@"public async Task<{tableNameSingularized}Response> GetByIdAsync({idType} id)
            {{
                string sql = @""SELECT {properties} 
                                FROM [{tableName}] 
                                WHERE Id = @Id"";

                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add(""@Id"", id);

                try
                {{
                    return await db.GetFirstAsync<{tableNameSingularized}Response>(sql, dynamicParameters);
                }}
                catch
                {{
                    throw;
                }}
            }}";
        }

        public static string GetAll(string tableName, string tableNameSingularized, string properties)
        {
            return $@"public async Task<IEnumerable<{tableNameSingularized}Response>> GetAllAsync()
            {{
                string sql = @""SELECT {properties} FROM [{tableName}]"";

                try
                {{
                    return await db.GetListAsync<{tableNameSingularized}Response>(sql, new());
                }}
                catch
                {{
                    throw;
                }}
            }} ";
        }

        public static string GetPaged()
        {
            return string.Empty;
        }
    }
}
