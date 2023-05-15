using BLL.General_Strings;
using BLL.Helpers;
using BLL.Models;
using BLL.Services.Interface;
using Microsoft.VisualBasic;
using Pluralize.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Logic
{
    public class StoredProcBuilderService : IStoredProcBuilderService
    {
        private readonly IFileService _fileService;
        private readonly Pluralizer _pluralizer;

        public StoredProcBuilderService(IFileService fileService)
        {
            _fileService = fileService;
            _pluralizer = new Pluralizer();
        }

        public Task BuildAsync(string tableName, List<ColumnModel> columnModels)
        {
            Task t1 = Task.Run(() => BuildInsertProc(tableName, columnModels));
            Task t2 = Task.Run(() => BuildUpdateProc(tableName, columnModels));

            Task.WaitAll(t1, t2);

            return Task.CompletedTask;
        }

        private async Task BuildInsertProc(string tableName, List<ColumnModel> columnModels)
        {
            List<string> columns = columnModels.Where(x => x.ColumnName?.ToLower() != "modified" && x.ColumnName?.ToLower() != "id" && x.ColumnName?.ToLower() != "datetimestamp" && x.ColumnName?.ToLower() != "isdeleted")
                .Select(x => $"[{x.ColumnName}]")
                .ToList();

            string columnsStr = string.Join(", ", columns);

            string proc = GeneralSql.StoredProcInsert(tableName, _pluralizer.Singularize(tableName), columnsStr);

            await _fileService.SaveFile("Database", "Stored_Procs", string.Empty, "sql", $"sp_{_pluralizer.Singularize(tableName)}_Insert", Formatter.FormatSql(proc));
        }

        private async Task BuildUpdateProc(string tableName, List<ColumnModel> columnModels)
        {
            StringBuilder stringBuilder = new();

            foreach (ColumnModel column in columnModels)
            {
                if (column.ColumnName?.ToLower() == "modified")
                {
                    stringBuilder.AppendLine($"t1.Modified = GETDATE()");
                }

                if (column.ColumnName?.ToLower() != "modified" && column.ColumnName?.ToLower() != "id" && column.ColumnName?.ToLower() != "datetimestamp")
                {
                    stringBuilder.AppendLine(GeneralSql.UpdateProperty(column.ColumnName ?? string.Empty));
                }
            }

            string updateStr = stringBuilder.ToString();
            updateStr = updateStr.Remove(updateStr.LastIndexOf(','));

            string proc = GeneralSql.StoredProcUpdate(tableName, _pluralizer.Singularize(tableName), updateStr);

            await _fileService.SaveFile("Database", "Stored_Procs", string.Empty, "sql", $"sp_{_pluralizer.Singularize(tableName)}_Update", Formatter.FormatSql(proc));
        }

        private async Task BuildPagedProc()
        {

        }
    }
}
