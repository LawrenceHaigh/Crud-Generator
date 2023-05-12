using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;

namespace Generator.Sample
{
    public class DatabaseSchema
    {
        public static void GetDatabaseSchema()
        {

            using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=Template;Integrated Security=True;"))
            {
                conn.Open();

                // Get the Meta Data for Supported Schema Collections
                DataTable metaDataTable = conn.GetSchema("MetaDataCollections");

                Console.WriteLine("Meta Data for Supported Schema Collections:");
                ShowDataTable(metaDataTable, 25);
                Console.WriteLine();

                // Get the schema information of Databases in your instance
                DataTable databasesSchemaTable = conn.GetSchema("Databases");

                Console.WriteLine("Schema Information of Databases:");
                ShowDataTable(databasesSchemaTable, 25);
                Console.WriteLine();

                // First, get schema information of all the tables in current database;
                DataTable allTablesSchemaTable = conn.GetSchema("Tables");

                Console.WriteLine("Schema Information of All Tables:");
                ShowDataTable(allTablesSchemaTable, 20);
                Console.WriteLine();

                // You can specify the Catalog, Schema, Table Name, Table Type to get
                // the specified table(s).
                // You can use four restrictions for Table, so you should create a 4 members array.
                String[] tableRestrictions = new String[4];

                // For the array, 0-member represents Catalog; 1-member represents Schema;
                // 2-member represents Table Name; 3-member represents Table Type.
                // Now we specify the Table Name of the table what we want to get schema information.
                tableRestrictions[2] = "Course";

                DataTable courseTableSchemaTable = conn.GetSchema("Tables", tableRestrictions);

                Console.WriteLine("Schema Information of Course Tables:");
                ShowDataTable(courseTableSchemaTable, 20);
                Console.WriteLine();

                // First, get schema information of all the columns in current database.
                DataTable allColumnsSchemaTable = conn.GetSchema("Columns");

                Console.WriteLine("Schema Information of All Columns:");
                ShowColumns(allColumnsSchemaTable);
                Console.WriteLine();

                // You can specify the Catalog, Schema, Table Name, Column Name to get the specified column(s).
                // You can use four restrictions for Column, so you should create a 4 members array.
                String[] columnRestrictions = new String[4];

                // For the array, 0-member represents Catalog; 1-member represents Schema;
                // 2-member represents Table Name; 3-member represents Column Name.
                // Now we specify the Table_Name and Column_Name of the columns what we want to get schema information.
                columnRestrictions[2] = "Logs";
                columnRestrictions[3] = "Id";

                DataTable departmentIDSchemaTable = conn.GetSchema("Columns", columnRestrictions);

                Console.WriteLine("Schema Information of DepartmentID Column in Course Table:");
                ShowColumns(departmentIDSchemaTable);
                Console.WriteLine();

                // First, get schema information of all the IndexColumns in current database
                DataTable allIndexColumnsSchemaTable = conn.GetSchema("IndexColumns");

                Console.WriteLine("Schema Information of All IndexColumns:");
                ShowIndexColumns(allIndexColumnsSchemaTable);
                Console.WriteLine();

                // You can specify the Catalog, Schema, Table Name, Constraint Name, Column Name to
                // get the specified column(s).
                // You can use five restrictions for Column, so you should create a 5 members array.
                String[] indexColumnsRestrictions = new String[5];

                // For the array, 0-member represents Catalog; 1-member represents Schema;
                // 2-member represents Table Name; 3-member represents Constraint Name;4-member represents Column Name.
                // Now we specify the Table_Name and Column_Name of the columns what we want to get schema information.
                indexColumnsRestrictions[2] = "Logs";
                indexColumnsRestrictions[4] = "Id";

                DataTable courseIdIndexSchemaTable = conn.GetSchema("IndexColumns", indexColumnsRestrictions);

                Console.WriteLine("Index Schema Information of CourseID Column in Course Table:");
                ShowIndexColumns(courseIdIndexSchemaTable);
                Console.WriteLine();
            }
        }

        private static void ShowDataTable(DataTable table, Int32 length)
        {
            foreach (DataColumn col in table.Columns)
            {
                Console.Write("{0,-" + length + "}", col.ColumnName);
            }
            Console.WriteLine();

            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn col in table.Columns)
                {
                    if (col.DataType.Equals(typeof(DateTime)))
                        Console.Write("{0,-" + length + ":d}", row[col]);
                    else if (col.DataType.Equals(typeof(Decimal)))
                        Console.Write("{0,-" + length + ":C}", row[col]);
                    else
                        Console.Write("{0,-" + length + "}", row[col]);
                }
                Console.WriteLine();
            }
        }

        private static void ShowColumns(DataTable columnsTable)
        {
            var selectedRows = from info in columnsTable.AsEnumerable()
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

            Console.WriteLine("{0,-15}{1,-15}{2,-25}{3,-30}{4,-25}{5,-15}{6,-15}{7,-20}{8,-15}",
                "TABLE_NAME", "COLUMN_NAME", "DATA_TYPE", "CHARACTER_MAXIMUM_LENGTH", "NUMERIC_PRECISION", "NUMERIC_SCALE", "IS_NULLABLE", "ORDINAL_POSITION", "COLUMN_DEFAULT");
            foreach (var row in selectedRows)
            {
                Console.WriteLine("{0,-15}{1,-15}{2,-25}{3,-30}{4,-25}{5,-15}{6,-15}{7,-20}{8,-15}",
                    row.TableName, row.ColumnName, row.DataType, row.MaxLength, row.NumericPrecision, row.PercisionFloatPoint, row.Nullable, row.OrdinalPosition, row.Default);
            }
        }

        private static void ShowIndexColumns(DataTable indexColumnsTable)
        {
            var selectedRows = from info in indexColumnsTable.AsEnumerable()
                               select new
                               {
                                   TableSchema = info["table_schema"],
                                   TableName = info["table_name"],
                                   ColumnName = info["column_name"],
                                   ConstraintSchema = info["constraint_schema"],
                                   ConstraintName = info["constraint_name"],
                                   KeyType = info["KeyType"]
                               };

            Console.WriteLine("{0,-14}{1,-11}{2,-14}{3,-18}{4,-35}{5,-15}", "table_schema", "table_name", "column_name", "constraint_schema", "constraint_name", "KeyType");
            foreach (var row in selectedRows)
            {
                Console.WriteLine("{0,-14}{1,-11}{2,-14}{3,-18}{4,-35}{5,-15}", row.TableSchema,
                    row.TableName, row.ColumnName, row.ConstraintSchema, row.ConstraintName, row.KeyType);
            }
        }
    }
}
