using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopToRelatedFile.Models
{
    public class File
    {
        public File(string fullPath)
        {
            this.FullPath = fullPath;
        }

        // Should only be touched by the DocumentService.  Separation of concerns.
        public Microsoft.VisualStudio.Text.ITextDocument TextDocument { get; set; }

        public string FullPath {get; set; }

        public override bool Equals(object obj)
        {
            return obj is File file &&
                   this.FullPath == file.FullPath;
        }

        public override int GetHashCode()
        {
            return 2018552787 + EqualityComparer<string>.Default.GetHashCode(this.FullPath);
        }
    }
}
