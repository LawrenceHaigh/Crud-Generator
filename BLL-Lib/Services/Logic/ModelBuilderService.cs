using BLL.General_Strings;
using BLL.Helpers;
using BLL.Models;
using BLL.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pluralize.NET.Core;

namespace BLL.Services.Logic
{
    public class ModelBuilderService : IModelBuilderService
    {
        private readonly IFileService _fileService;

        public ModelBuilderService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task BuildAsync(string tableName, List<ColumnModel> columnModels)
        {
            Pluralizer pluralizer = new Pluralizer();

            string tableNameSingularized = pluralizer.Singularize(tableName);

            StringBuilder request = new();
            StringBuilder response = new();

            foreach (ColumnModel columnModel in columnModels)
            {
                string property = await BuildProperties(columnModel);

                if (columnModel.ColumnName?.ToLower() != "id" && columnModel.ColumnName?.ToLower() != "datetimestamp")
                {
                    request.Append(property);
                    request.AppendLine();
                }

                response.Append(property);
                response.AppendLine();
            }

            await _fileService.SaveFile("Models", "Contracts", tableName, "cs", $"{tableName}Request", Formatter.FormatCSharp(GeneralCSharp.ModelClass($"Models.Contracts.{tableName}", $"{tableNameSingularized}Request", request.ToString())));
            await _fileService.SaveFile("Models", "Contracts", tableName, "cs", $"{tableName}Response", Formatter.FormatCSharp(GeneralCSharp.ModelClass($"Models.Contracts.{tableName}", $"{tableNameSingularized}Response", response.ToString())));
        }

        private async Task<string> BuildProperties(ColumnModel columnModel)
        {
            Type type = await ConvertTypes.GetVariableTypeAsync(columnModel.DataType ?? string.Empty, columnModel.PercisionFloatPoint ?? string.Empty, columnModel.Nullable ?? string.Empty);

            return GeneralCSharp.ModelPropery(columnModel.ColumnName ?? string.Empty, type.GetCSharpName());
        }
    }
}
