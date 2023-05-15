using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.General_Strings
{
    public class GeneralSql
    {
        public static string UserDefinedType(string tableNameSingularized, string operation, string properties)
        {
            return @$"CREATE TYPE udt_{tableNameSingularized}_{operation} AS TABLE 
            ( 
                {properties}
            );";
        }

        public static string UserDefinedTypeProperty(string columnName, string type, string nullable) =>
            @$"[{columnName}] {type} {nullable},";

        public static string StoredProcInsert(string tableName, string tableNameSingularized, string columns) =>
            $@"CREATE PROCEDURE [dbo].[sp_{tableNameSingularized}_Insert]
	            @{tableNameSingularized} udt_{tableNameSingularized}_Insert readonly
            AS
            BEGIN
                INSERT INTO [{tableName}]({columns})
	            SELECT {columns}
	            FROM @{tableName};
            END";

        public static string StoredProcUpdate(string tableName, string tableNameSingularized, string script) =>
            $@"CREATE PROCEDURE [dbo].[sp_{tableNameSingularized}_Update]
                @{tableNameSingularized} udt_{tableNameSingularized}_Update readonly
            AS
            BEGIN
                UPDATE t1
                SET {script}
                FROM [{tableName}] AS t1
                INNER JOIN @{tableName} t2 ON t1.[Id] = t2.[Id];
            END";

        public static string UpdateProperty(string columnName) =>
            @$"t1.[{columnName}] = t2.[{columnName}],";
    }
}
