
namespace PopToRelatedFile.Services
{
    public interface IPopNextService
    {
        Task GoToNextFileAsync();
        Task ResetOriginAsync();
    }
}