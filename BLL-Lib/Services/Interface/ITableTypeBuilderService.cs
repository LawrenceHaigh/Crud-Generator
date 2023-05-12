using BLL.Models;

namespace BLL.Services.Interface
{
    public interface ITableTypeBuilderService
    {
        Task BuildUserDefinedTypesAsync(string tableName, List<ColumnModel> columnModels);
    }
}