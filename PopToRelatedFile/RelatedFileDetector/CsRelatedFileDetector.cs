using Microsoft.VisualStudio.Text;
using PopToRelatedFile.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopToRelatedFile
{
    public class CsRelatedFileDetector : IRelatedFileDetector
    {
        IDocumentService documentService;

        public CsRelatedFileDetector(IDocumentService documentService)
        {
            this.documentService = documentService;
        }

        public async Task<List<string>> CorrespondingFiles(ITextDocument document)
        {
            return this.CorrespondingCshtmlFiles(await this.documentService.FilePath(document));
        }

        public async Task<bool> IsType(ITextDocument document)
        {
            var path = await documentService.FilePath(document);
            return documentService.IsType(document, "CSharp") && !path.EndsWith(".cshtml");
        }


        private List<string> CorrespondingCshtmlFiles(string filePath)
        {

            var cshtmlPath = this.CshtmlPath(filePath);
            if (documentService.FileExists(cshtmlPath))
            {
                return new List<string> { cshtmlPath };
            }
            
            return new List<string>();
        }

        private string CshtmlPath(string filePath)
        {
            return filePath.Substring(0, filePath.Length - ".cs".Length);
        }

    }
}
