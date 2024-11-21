using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopToRelatedFile.Services
{
    public class DocumentService : IDocumentService
    {
        public bool FileExists(string filePath) =>
            File.Exists(filePath);

        public async Task<string> FilePath(ITextDocument document)
        {
            var documentView = await VS.Documents.GetActiveDocumentViewAsync();
            var path = documentView?.Document?.FilePath;
            if (path == null)
            {
                return null;
            }
            return path;
        }

        public bool IsType(ITextDocument document, string type) =>
            document.TextBuffer.ContentType.IsOfType(type);

        public string GetDocumentText(ITextDocument document)
        {
            return document.TextBuffer.CurrentSnapshot.GetText();
        }


    }
}
