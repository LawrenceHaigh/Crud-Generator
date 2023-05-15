using BLL.General_Strings;
using BLL.Helpers;
using BLL.Models;
using BLL.Services.Interface;
using Pluralize.NET.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Logic
{
    public class RepositoryBuilderService : IRepositoryBuilderService
    {
        private readonly IFileService _fileService;
        private readonly TextInfo culture = new CultureInfo("en-ZA", false).TextInfo;

        public RepositoryBuilderService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task BuildAsync(string tableName, List<ColumnModel> columnModels)
        {
            Pluralizer pluralizer = new();

            string TableNameSingularized = pluralizer.Singularize(tableName);
            string TableNameTitleCaseSingularized = pluralizer.Singularize(culture.ToTitleCase(tableName ?? string.Empty));

            List<string> columns = columnModels.Where(x => x.ColumnName?.ToLower() != "modified" && x.ColumnName?.ToLower() != "id" && x.ColumnName?.ToLower() != "datetimestamp" && x.ColumnName?.ToLower() != "isdeleted")
                .Select(x => $"[{x.ColumnName}]")
                .ToList();

            string columnsStr = string.Join(", ", columns);

            StringBuilder stringBuilder = new();

            ColumnModel? idColumn = columnModels.Where(x => x.ColumnName?.ToLower() == "id").FirstOrDefault();

            string idType = string.Empty;

            if (idColumn != null)
            {
                Type type = await ConvertTypes.GetVariableTypeAsync(idColumn.DataType ?? string.Empty, idColumn.PercisionFloatPoint ?? string.Empty, idColumn.Nullable ?? string.Empty);
                idType = type.GetCSharpName();

                stringBuilder.AppendLine(DeleteAsync(tableName, idType));
                stringBuilder.AppendLine(GetByIdAsync(tableName, TableNameSingularized, columnsStr, idType));
            }

            stringBuilder.AppendLine(CreateAsync(tableName, TableNameSingularized, TableNameTitleCaseSingularized));
            stringBuilder.AppendLine(UpdateAsync(tableName, TableNameSingularized, TableNameTitleCaseSingularized));            
            stringBuilder.AppendLine(GetAllAsync(tableName, TableNameSingularized, columnsStr));
            stringBuilder.AppendLine(GetPagedAsync(tableName));

            string repositoryClass = GeneralCSharp.RepositoryClass("BLL.Repositories.Logic", "Models", "BLL", tableName, TableNameSingularized, stringBuilder.ToString());

            await _fileService.SaveFile("BLL", "Repositories", "Logic", "cs", $"{TableNameSingularized}Repository", Formatter.FormatCSharp(repositoryClass));
            await _fileService.SaveFile("BLL", "Repositories", "Interfaces", "cs", $"I{TableNameSingularized}Repository", Formatter.FormatCSharp(GeneralCSharp.RepositoryInterface("BLL.Repositories.Interfaces", "Models", tableName, TableNameSingularized, idType)));
        }

        private static string CreateAsync(string tableName, string TableNameSingularized, string TableNameTitleCaseSingularized)
        {
            return Functions.Create(tableName, TableNameSingularized, TableNameTitleCaseSingularized);
        }

        private static string UpdateAsync(string tableName, string TableNameSingularized, string TableNameTitleCaseSingularized)
        {
            return Functions.Update(tableName, TableNameSingularized, TableNameTitleCaseSingularized);
        }

        private static string DeleteAsync(string tableName, string idType)
        {
            return Functions.Delete(tableName, idType);
        }

        private static string GetByIdAsync(string tableName, string TableNameSingularized, string properties, string idType)
        {
            return Functions.GetById(tableName, TableNameSingularized, properties, idType);
        }

        private static string GetAllAsync(string tableName, string TableNameSingularized, string properties)
        {
            return Functions.GetAll(tableName, TableNameSingularized, properties);
        }

        private static string GetPagedAsync(string tableName)
        {
            return Functions.GetPaged();
        }
    }
}
