using BLL.General_Strings;
using BLL.Helpers;
using BLL.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Logic
{
    public class SqlAccessBuilderService : ISqlAccessBuilderService
    {
        private readonly IFileService _fileService;

        public SqlAccessBuilderService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public Task BuildDapperConnectorAsync(string assemblyName, string folderName)
        {
            Task classStr = Task.Run(() => BuildDapperConnectorClassAsync(assemblyName, folderName));
            Task interfaceStr = Task.Run(() => BuildDapperConnectorInterfaceAsync(assemblyName, folderName));

            Task.WaitAll(classStr, interfaceStr);

            return Task.CompletedTask;
        }

        private async Task BuildDapperConnectorClassAsync(string assemblyName, string folderName)
        {
            StringBuilder stringBuilder = new();
            StringBuilder contentStr = new();

            stringBuilder.Append(DapperConnector.Using());
            stringBuilder.AppendLine();

            contentStr.Append(DapperConnector.Properties());
            contentStr.AppendLine();
            contentStr.Append(DapperConnector.Constructor());
            contentStr.AppendLine();
            contentStr.Append(DapperConnector.Connector());
            contentStr.AppendLine();
            contentStr.Append(DapperConnector.GetFirstAsync());
            contentStr.AppendLine();
            contentStr.Append(DapperConnector.GetListAsync());
            contentStr.AppendLine();
            contentStr.Append(DapperConnector.ExecuteAsync());

            string classStr = GeneralCSharp.Class("public", DapperConnector.FileName(), contentStr.ToString());

            stringBuilder.Append(GeneralCSharp.ClassNameSpace(assemblyName, folderName, classStr));

            await _fileService.SaveFile(assemblyName, folderName, string.Empty, "cs", DapperConnector.FileName(), Formatter.FormatCSharp(stringBuilder.ToString()));
        }

        private async Task BuildDapperConnectorInterfaceAsync(string assemblyName, string folderName)
        {
            StringBuilder stringBuilder = new();

            stringBuilder.Append(DapperConnector.Using());
            stringBuilder.AppendLine();

            string interfaceStr = GeneralCSharp.Interface("public", DapperConnector.FileName(), DapperConnector.Interface());

            stringBuilder.Append(GeneralCSharp.InterfaceNameSpace(assemblyName, folderName, interfaceStr));

            await _fileService.SaveFile(assemblyName, folderName, string.Empty, "cs", $"I{DapperConnector.FileName()}", Formatter.FormatCSharp(stringBuilder.ToString()));
        }
    }
}
