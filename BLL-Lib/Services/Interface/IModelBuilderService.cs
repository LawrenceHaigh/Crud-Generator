using BLL.Models;

namespace BLL.Services.Interface
{
    public interface IModelBuilderService
    {
        Task BuildModelsAsync(string tableName, List<ColumnModel> columnModels);
    }
}