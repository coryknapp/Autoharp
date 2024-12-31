
namespace Autoharp.Services
{
    public interface IPopNextService
    {
        Task GoToNextFileAsync();
        Task ResetOriginAsync();
    }
}