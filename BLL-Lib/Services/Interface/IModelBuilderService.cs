﻿using BLL.Models;

namespace BLL.Services.Interface
{
    public interface IModelBuilderService
    {
        Task BuildAsync(string tableName, List<ColumnModel> columnModels);
    }
}