using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Services.Interface;

namespace BLL.Services.Logic
{
    public class FileService : IFileService
    {
        private readonly string _fileLocation;

        public FileService()
        {
            _fileLocation = "C:/Users/lawre/source/repos/Crud-Generator/Generator/FileDrop";
        }

        public async Task SaveFile(string assembly, string folder, string subFolder, string fileType, string fileName, string content)
        {
            string path = _fileLocation;

            if (string.IsNullOrEmpty(subFolder))
            {
                path += $"/{assembly}/{folder}";
            }
            else
            {
                path += $"/{assembly}/{folder}/{subFolder}";
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += $"/{fileName}.{fileType}";

            using StreamWriter writer = new(path);

            await writer.WriteAsync(content);
        }

        public void ClearDirectory()
        {
            if (Directory.Exists(_fileLocation))
            {
                Directory.Delete(_fileLocation, true);
            }
        }
    }
}
