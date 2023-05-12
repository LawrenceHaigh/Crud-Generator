namespace BLL.Services.Interface
{
    public interface IFileService
    {
        Task SaveFile(string assembly, string folder, string subFolder, string fileType, string fileName, string content);
        void ClearDirectory();
    }
}