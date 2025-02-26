using Microsoft.VisualStudio.Text;
using Autoharp.Models;
using Autoharp.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autoharp
{
    public class CsToCshtmlFileDetector : IRelatedFileDetector
    {
        IVsSolutionService documentService;

        public CsToCshtmlFileDetector(IVsSolutionService documentService)
        {
            this.documentService = documentService;
        }

        public async Task<IEnumerable<File>> CorrespondingFilesAsync(File file) =>
            await Task.Run(() => this.CorrespondingCshtmlFiles(file));

        public async Task<bool> IsTypeAsync(File file) =>
            await documentService.IsTypeAsync(file, "CSharp")
                && !file.FullPath.EndsWith(".cshtml");


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
