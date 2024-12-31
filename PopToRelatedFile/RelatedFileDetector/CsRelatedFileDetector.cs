﻿using Microsoft.VisualStudio.Text;
using PopToRelatedFile.Models;
using PopToRelatedFile.Services;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<File>> CorrespondingFilesAsync(File file)
        {
            return await Task.Run(() => this.CorrespondingCshtmlFiles(file));
        }

        public async Task<bool> IsTypeAsync(File file) =>
            await documentService.IsTypeAsync(file, "CSharp") && !file.FullPath.EndsWith(".cshtml");


        private IEnumerable<File> CorrespondingCshtmlFiles(File file)
        {
            var cshtmlFile = this.CshtmlFile(file);
            if (documentService.FileExists(cshtmlFile))
            {
                return new List<File> { cshtmlFile };
            }
            
            return Enumerable.Empty<File>();
        }

        private File CshtmlFile(File file) =>
            new File(file.FullPath.Substring(0, file.FullPath.Length - 3));
    }
}
