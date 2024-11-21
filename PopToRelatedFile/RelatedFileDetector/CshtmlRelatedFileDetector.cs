using Microsoft.VisualStudio.Text;
using PopToRelatedFile.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopToRelatedFile
{
    public class CshtmlRelatedFileDetector : IRelatedFileDetector
    {
        IDocumentService documentService;

        public CshtmlRelatedFileDetector(IDocumentService documentService)
        {
            this.documentService = documentService;
        }

        public async Task<List<string>> CorrespondingFiles(ITextDocument document)
        {
            return this.CorrespondingCsFiles(await documentService.FilePath(document));
        }

        public async Task<bool> IsType(ITextDocument document)
        {
            var path = await documentService.FilePath(document);
            return path.EndsWith(".cshtml");
        }

        private List<string> CorrespondingCsFiles(string filePath)
        {
            var cshtmlPath = Path.ChangeExtension(filePath, "cshtml.cs");
            if (documentService.FileExists(cshtmlPath))
            {
                return new List<string> { cshtmlPath };
            }
            
            return new List<string>();
        }

    }
}
