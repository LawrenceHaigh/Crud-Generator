using BLL.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Helpers
{
    public static class ConvertTypes
    {
        public static Task<Type> GetVariableTypeAsync(string sqlType, string dataPointStr, string nullableStr)
        {
            _ = int.TryParse(dataPointStr, out int dataPoint);
            bool nullable = false;

            if (nullableStr.ToLower() == "yes")
            {
                nullable = true;
            }

            DataTypes enumType = (DataTypes)Enum.Parse(typeof(DataTypes), sqlType, true);

            Type type = enumType switch
            {
                DataTypes.bigint => nullable ? typeof(Int64?) : typeof(Int64),
                DataTypes.@int => nullable ? typeof(int?) : typeof(int),
                DataTypes.tinyint => nullable ? typeof(Int16?) : typeof(Int16),
                DataTypes.smallint => nullable ? typeof(Int16?) : typeof(Int16),
                DataTypes.@float => nullable ? typeof(float?) : typeof(float),
                DataTypes.@decimal => GetIntTypeByFloatPoint(dataPoint, nullable),
                DataTypes.numeric => GetIntTypeByFloatPoint(dataPoint, nullable),
                DataTypes.money => nullable ? typeof(decimal?) : typeof(decimal),
                DataTypes.smallmoney => nullable ? typeof(decimal?) : typeof(decimal),
                DataTypes.@char => GetTypeByMaxLength(dataPoint, nullable),
                DataTypes.nchar => GetTypeByMaxLength(dataPoint, nullable),
                DataTypes.ntext => typeof(string),
                DataTypes.nvarchar => typeof(string),
                DataTypes.varchar => typeof(string),
                DataTypes.text => typeof(string),
                DataTypes.date => nullable ? typeof(DateTime?) : typeof(DateTime),
                DataTypes.datetime => nullable ? typeof(DateTime?) : typeof(DateTime),
                DataTypes.datetime2 => nullable ? typeof(DateTime?) : typeof(DateTime),
                DataTypes.smalldatetime => nullable ? typeof(DateTime?) : typeof(DateTime),
                DataTypes.datetimeoffset => nullable ? typeof(TimeSpan?) : typeof(TimeSpan),
                DataTypes.time => nullable ? typeof(TimeSpan?) : typeof(TimeSpan),
                DataTypes.timestamp => nullable ? typeof(TimeSpan?) : typeof(TimeSpan),
                DataTypes.bit => nullable ? typeof(bool?) : typeof(bool),
                DataTypes.uniqueidentifier => nullable ? typeof(Guid?) : typeof(Guid),
                DataTypes.binary => typeof(byte[]),
                DataTypes.geography => typeof(string),
                DataTypes.geometry => typeof(string),
                DataTypes.hierarchyid => typeof(string),
                DataTypes.image => typeof(string),
                DataTypes.real => typeof(string),
                DataTypes.sql_variant => typeof(string),
                DataTypes.varbinary => typeof(byte[]),
                DataTypes.xml => typeof(string),
                _ => typeof(string),
            };

            return Task.FromResult(type);
        }

        public static string RebuildSqlType(string type, string floatPoint, string maxLength, string Precision)
        {
            string sqlType = type;

            if (type == "nvarchar" || type == "varchar" || type == "nchar" || type == "char" || type == "decimal")
            {
                sqlType = type switch
                {
                    "nvarchar" => $"nvarchar({(maxLength == "-1" ? "max" : maxLength)})",
                    "varchar" => $"varchar({(maxLength == "-1" ? "max" : maxLength)})",
                    "nchar" => $"nchar({(maxLength == "-1" ? "max" : maxLength)})",
                    "char" => $"char({(maxLength == "-1" ? "max" : maxLength)})",
                    "decimal" => $"decimal({Precision},{floatPoint})",
                    _ => throw new NotImplementedException()
                };
            }

            return sqlType;
        }

        private static Type GetIntTypeByFloatPoint(int dataPoint, bool nullable)
        {
            if (dataPoint > 17)
            {
                return nullable ? typeof(decimal?) : typeof(decimal);
            }
            else
            {
                return nullable ? typeof(double?) : typeof(double);
            }
        }

        private static Type GetTypeByMaxLength(int dataPoint, bool nullable)
        {
            if (dataPoint > 1)
            {
                return typeof(string);
            }
            else
            {
                return nullable ? typeof(char?) : typeof(char);
            }
        }
    }
}
