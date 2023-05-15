using BLL.Models;

namespace BLL.Services.Interface
{
    public interface IRepositoryBuilderService
    {
        Task BuildAsync(string tableName, List<ColumnModel> columnModels);
    }
}