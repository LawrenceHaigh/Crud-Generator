using BLL.Models;
using BLL.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Logic
{
    public class SqlSchemaService : ISqlSchemaService
    {
        public List<TableModel> GetTableProperties()
        {
            using SqlConnection conn = new("Data Source=.;Initial Catalog=Template;Integrated Security=True;");
            conn.Open();

            DataTable allColumnsSchemaTable = conn.GetSchema("Columns");

            return ConvertData(allColumnsSchemaTable);
        }

        private List<TableModel> ConvertData(DataTable allColumnsSchemaTable)
        {
            IEnumerable<TableDataModel> selectedRows = from info in allColumnsSchemaTable.AsEnumerable()
                                                       select new TableDataModel
                                                       {
                                                           TableName = info["TABLE_NAME"].ToString(),
                                                           ColumnName = info["COLUMN_NAME"].ToString(),
                                                           DataType = info["DATA_TYPE"].ToString(),
                                                           MaxLength = info["CHARACTER_MAXIMUM_LENGTH"].ToString(),
                                                           NumericPrecision = info["NUMERIC_PRECISION"].ToString(),
                                                           PercisionFloatPoint = info["NUMERIC_SCALE"].ToString(),
                                                           Nullable = info["IS_NULLABLE"].ToString(),
                                                           OrdinalPosition = info["ORDINAL_POSITION"].ToString(),
                                                           Default = info["COLUMN_DEFAULT"].ToString()
                                                       };

            return selectedRows.GroupBy(p => p.TableName, p => p, (key, g) => new TableModel
            {
                TableName = key,
                Columns = g.Select(x => new ColumnModel
                {
                    ColumnName = x.ColumnName,
                    DataType = x.DataType,
                    Default = x.Default,
                    MaxLength = x.MaxLength,
                    NumericPrecision = x.NumericPrecision,
                    PercisionFloatPoint = x.PercisionFloatPoint,
                    Nullable = x.Nullable,
                    OrdinalPosition = x.OrdinalPosition
                })
                .OrderBy(x => x.OrdinalPosition)
                .ToList()
            })
            .ToList();
        }
    }
}
