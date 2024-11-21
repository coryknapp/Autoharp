using Microsoft.VisualStudio.Text;
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
        Task<List<string>> CorrespondingFiles(ITextDocument document);

        Task<bool> IsType(ITextDocument document);
    }
}
