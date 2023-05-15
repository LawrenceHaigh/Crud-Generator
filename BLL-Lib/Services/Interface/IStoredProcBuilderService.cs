using BLL.Models;

namespace BLL.Services.Logic
{
    public interface IStoredProcBuilderService
    {
        Task BuildAsync(string tableName, List<ColumnModel> columnModels);
    }
}