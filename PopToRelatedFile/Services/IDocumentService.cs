using Microsoft.VisualStudio.Text;
using System.Threading.Tasks;

namespace PopToRelatedFile.Services
{
    public interface IDocumentService
    {
        bool FileExists(string filePath);

        Task<string> FilePath(ITextDocument document);

        bool IsType(ITextDocument document, string type);

        string GetDocumentText(ITextDocument document);
    }
}