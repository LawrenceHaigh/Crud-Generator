using BLL.General_Strings;
using BLL.Helpers;
using BLL.Models;
using BLL.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Logic
{
    public class APIBuilderService : IAPIBuilderService
    {
        private readonly IFileService _fileService;
        private readonly Endpoints _endpoints;
        private string TableName { get; set; }

        public APIBuilderService(IFileService fileService, string tableName)
        {
            _fileService = fileService;
            TableName = tableName;
            _endpoints = new Endpoints(tableName);
        }

        public async Task BuildAsync(List<ColumnModel> columnModels)
        {
            ColumnModel? idColumn = columnModels.Where(x => x.ColumnName?.ToLower() == "id").FirstOrDefault();

            if (idColumn != null)
            {
                Type type = await ConvertTypes.GetVariableTypeAsync(idColumn.DataType ?? string.Empty, idColumn.PercisionFloatPoint ?? string.Empty, idColumn.Nullable ?? string.Empty);
                string idType = type.GetCSharpName();

                Task t1 = Task.Run(() => UpdateAsync(idType));
                Task t2 = Task.Run(() => DeleteAsync(idType));
                Task t3 = Task.Run(() => GetByIdAsync(idType));

                await Task.WhenAll(t1, t2, t3);
            }

            Task t4 = Task.Run(() => CreateAsync());            
            Task t5 = Task.Run(() => GetAllAsync());
            Task t6 = Task.Run(() => GetPagedAsync());

            await Task.WhenAll(t4, t5, t6);
        }

        private async Task CreateAsync()
        {
            await _fileService.SaveFile("API", "Endpoints", TableName, "cs", $"CreateEndpoint", Formatter.FormatCSharp(_endpoints.Create("API", "Models", "Repositories")));
        }

        private async Task UpdateAsync(string idType)
        {
            await _fileService.SaveFile("API", "Endpoints", TableName, "cs", $"UpdateEndpoint", Formatter.FormatCSharp(_endpoints.Update("API", "Models", "Repositories", idType)));
        }

        private async Task DeleteAsync(string idType)
        {
            await _fileService.SaveFile("API", "Endpoints", TableName, "cs", $"DeleteEndpoint", Formatter.FormatCSharp(_endpoints.Delete("API", "Repositories", idType)));
        }

        private async Task GetByIdAsync(string idType)
        {
            await _fileService.SaveFile("API", "Endpoints", TableName, "cs", $"GetByIdEndpoint", Formatter.FormatCSharp(_endpoints.GetById("API", "Models", "Repositories", idType)));
        }

        private async Task GetAllAsync()
        {
            await _fileService.SaveFile("API", "Endpoints", TableName, "cs", $" GetAllEndpoint", Formatter.FormatCSharp(_endpoints.GetAll("API", "Models", "Repositories")));
        }

        private async Task GetPagedAsync()
        {
            await _fileService.SaveFile("API", "Endpoints", TableName, "cs", $"GetPagedEndpoint", Formatter.FormatCSharp(_endpoints.GetPaged("API", "Models", "Repositories")));
        }
    }
}
