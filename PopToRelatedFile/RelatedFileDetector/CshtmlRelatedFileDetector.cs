using Microsoft.VisualStudio.Text;
using PopToRelatedFile.Models;
using PopToRelatedFile.Services;
using System;
using System.Collections;
using System.Collections.Generic;
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

        public async Task<IEnumerable<File>> CorrespondingFilesAsync(File file)
        {
            return this.CorrespondingCsFiles(file);
        }

        public async Task<bool> IsTypeAsync(File file) =>
            file.FullPath.EndsWith(".cshtml");

        private IEnumerable<File> CorrespondingCsFiles(File file)
        {
            var cshtml = this.GetFileWithChangedExtension(file);
            if (documentService.FileExists(cshtml))
            {
                return new List<File> { cshtml };
            }
            
            return Enumerable.Empty<File>();
        }

        private File GetFileWithChangedExtension(File file) =>
            new File($"{file.FullPath}.cs");
    }
}
