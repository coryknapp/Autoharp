
using System.Threading.Tasks;

namespace Autoharp.Services
{
    public interface IJumpNextService
    {
        Task GoToNextFileAsync();
        Task ResetOriginAsync();
        Task<bool> IsActiveFileInJumpListAsync();
        Task AddActiveFileToJumpListAsyncAsync();
        Task RemoveActiveFileFromJumpListAsync();
    }
}