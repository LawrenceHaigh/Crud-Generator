using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.General_Strings
{
    public class GeneralSql
    {
        public static string UserDefinedType(string tableName, string operation, string properties)
        {
            return @$"CREATE TYPE udt_{tableName}_{operation} AS TABLE 
( 
    {properties}
);";
        }

        public static string UserDefinedTypeProperty(string columnName, string type, string nullable)
        {
            return @$"[{columnName}] {type} {nullable},";
        }
    }
}
