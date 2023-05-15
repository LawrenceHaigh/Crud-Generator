using BLL.General_Strings;
using BLL.Helpers;
using BLL.Models;
using BLL.Services.Interface;
using Pluralize.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Logic
{
    public class TableTypeBuilderService : ITableTypeBuilderService
    {
        private readonly IFileService _fileService;
        private readonly Pluralizer _pluralizer;

        public TableTypeBuilderService(IFileService fileService)
        {
            _fileService = fileService;
            _pluralizer = new Pluralizer();
        }

        public async Task BuildAsync(string tableName, List<ColumnModel> columnModels)
        {
            StringBuilder insert = new();
            StringBuilder update = new();

            foreach (ColumnModel columnModel in columnModels)
            {
                string property = BuildProperties(columnModel);

                if (columnModel.ColumnName?.ToLower() != "id" && columnModel.ColumnName?.ToLower() != "datetimestamp" && columnModel.ColumnName?.ToLower() != "modified" && columnModel.ColumnName?.ToLower() != "isdeleted")
                {
                    insert.Append(property);
                    insert.AppendLine();
                }

                update.Append(property);
                update.AppendLine();
            }

            string insertStr = insert.ToString();
            insertStr = insertStr.Remove(insertStr.LastIndexOf(','));

            string updateStr = update.ToString();
            updateStr = updateStr.Remove(updateStr.LastIndexOf(','));

            await _fileService.SaveFile("Database", "User_Defined_Types", string.Empty, "sql", $"udt_{_pluralizer.Singularize(tableName)}_Insert", Formatter.FormatSql(GeneralSql.UserDefinedType(_pluralizer.Singularize(tableName), "Insert", insertStr)));
            await _fileService.SaveFile("Database", "User_Defined_Types", string.Empty, "sql", $"udt_{_pluralizer.Singularize(tableName)}_Update", Formatter.FormatSql(GeneralSql.UserDefinedType(_pluralizer.Singularize(tableName), "Update", updateStr)));
        }

        private string BuildProperties(ColumnModel columnModel)
        {
            string nullable = "null";

            if (columnModel.Nullable?.ToLower() == "no")
            {
                nullable = "not null";
            }

            return GeneralSql.UserDefinedTypeProperty(columnModel.ColumnName ?? string.Empty, ConvertTypes.RebuildSqlType(columnModel.DataType ?? string.Empty, columnModel.PercisionFloatPoint ?? string.Empty, columnModel.MaxLength ?? string.Empty, columnModel.NumericPrecision ?? string.Empty), nullable);
        }
    }
}
