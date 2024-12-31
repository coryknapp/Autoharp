using PopToRelatedFile.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PopToRelatedFile.Services
{
    public interface IDocumentService
    {
        bool FileExists(File file);

        Task<File> GetCurrentFileAsync();

        File GetFileForDocumentView(DocumentView documentView);

        Task<bool> IsTypeAsync(File file, string type);

        Task<string> GetDocumentTextAsync(File file);

        Task<IEnumerable<File>> GetAllFilesAsync(Func<File, bool> filter = null);
    }
}