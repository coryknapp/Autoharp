using System;
using System.Collections.Generic;
using System.Linq;
using Autoharp.Models;
using System.Text;
using System.Threading.Tasks;

namespace Autoharp.Services
{
    public interface IVsSolutionService
    {
        bool FileExists(File file);

        Task<File> GetCurrentFileAsync();

        Task<bool> IsTypeAsync(File file, string type);

        Task<string> GetDocumentTextAsync(File file);

        Task<IEnumerable<File>> GetAllFilesAsync(Func<File, bool> filter = null);

        IEnumerable<Token> GetClasses(File file);

        IEnumerable<Token> GetClassAncestors(Token classToken);

        bool IsTokenUserDefined(Token c);

        File GetFile(Token c);
    }
}
