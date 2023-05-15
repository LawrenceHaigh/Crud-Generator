using BLL.Models;
using BLL.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Logic
{
    public class RawSqlBuilderService
    {
        private readonly IFileService _fileService;

        public RawSqlBuilderService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<List<string>> BuildAsync(string tableName, List<ColumnModel> columnModels)
        {
            return new();
        }

        private Task<string> GetById()
        {
            return Task.Run(() => string.Empty);
        }

        private Task<string> GetAll()
        {
            return Task.Run(() => string.Empty);
        }

        private Task<string> GetPaged()
        {
            return Task.Run(() => string.Empty);
        }
    }
}