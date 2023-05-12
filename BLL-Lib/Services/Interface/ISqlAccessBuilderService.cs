namespace BLL.Services.Interface
{
    public interface ISqlAccessBuilderService
    {
        Task BuildDapperConnectorAsync(string assemblyName, string folderName);
    }
}