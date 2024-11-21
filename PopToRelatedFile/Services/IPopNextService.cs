
namespace PopToRelatedFile.Services
{
    public interface IPopNextService
    {
        Task GoToNextFile();
        Task ResetOrigin();
    }
}