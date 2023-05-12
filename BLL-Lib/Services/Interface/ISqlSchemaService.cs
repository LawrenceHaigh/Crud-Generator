using BLL.Models;

namespace BLL.Services.Interface
{
    public interface ISqlSchemaService
    {
        List<TableModel> GetTableProperties();
    }
}