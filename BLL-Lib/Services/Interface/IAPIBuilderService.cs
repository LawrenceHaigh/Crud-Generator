using BLL.Models;

namespace BLL.Services.Interface
{
    public interface IAPIBuilderService
    {
        Task BuildAsync(List<ColumnModel> columnModels);
    }
}