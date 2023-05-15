using BLL.Models;

namespace BLL.Services.Interface
{
    public interface ITableTypeBuilderService
    {
        Task BuildAsync(string tableName, List<ColumnModel> columnModels);
    }
}