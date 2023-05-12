﻿using BLL.Models;
using BLL.Services.Interface;
using BLL.Services.Logic;
using Generator.Sample;
using System.Data;
using System.Data.SqlClient;

namespace Generator
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IFileService fileService = new FileService();

            fileService.ClearDirectory();

            DatabaseSchema.GetDatabaseSchema();

            ISqlSchemaService sqlSchemaService = new SqlSchemaService();
            IModelBuilderService modelBuilderService = new ModelBuilderService(fileService);
            ISqlAccessBuilderService sqlAccessBuilderService = new SqlAccessBuilderService(fileService);
            ITableTypeBuilderService tableTypeBuilderService = new TableTypeBuilderService(fileService);

            await sqlAccessBuilderService.BuildDapperConnectorAsync("BLL", "Data");

            foreach (TableModel data in sqlSchemaService.GetTableProperties())
            {
                await modelBuilderService.BuildModelsAsync(data.TableName ?? string.Empty, data.Columns);
                //Fix script Styling.
                await tableTypeBuilderService.BuildUserDefinedTypesAsync(data.TableName ?? string.Empty, data.Columns);
            }

            Console.WriteLine("Please press any key to exit...");
            Console.ReadKey();
        }
    }
}