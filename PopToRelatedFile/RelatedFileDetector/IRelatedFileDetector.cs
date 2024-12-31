using Microsoft.VisualStudio.Text;
using PopToRelatedFile.Models;
using PopToRelatedFile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopToRelatedFile
{
    public interface IRelatedFileDetector
    {
        Task<IEnumerable<File>> CorrespondingFilesAsync(File document);

        Task<bool> IsTypeAsync(File document);
    }
}
