using Autoharp.Models;
using Autoharp.Services;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Autoharp
{
    public class CsClassAncestorsDetector : IRelatedFileDetector
    {
        IVsSolutionService soluitionService;

        public CsClassAncestorsDetector(IVsSolutionService soluitionService)
        {
            this.soluitionService = soluitionService;
        }

        public async Task<IEnumerable<File>> CorrespondingFilesAsync(File file) =>
            soluitionService.GetClasses(file)
                .SelectMany(c => this.soluitionService.GetClassAncestors(c))
                .Where(c => soluitionService.IsTokenUserDefined(c))
                .Select(c => soluitionService.GetFile(c));

        public async Task<bool> IsTypeAsync(File file) =>
            await soluitionService.IsTypeAsync(file, "CSharp")
                && !file.FullPath.EndsWith(".cshtml");
    }
}
