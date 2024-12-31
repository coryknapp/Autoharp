using Microsoft.VisualStudio.Text;
using Autoharp.Models;
using Autoharp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoharp
{
    public interface IRelatedFileDetector
    {
        Task<IEnumerable<File>> CorrespondingFilesAsync(File document);

        Task<bool> IsTypeAsync(File document);
    }
}
