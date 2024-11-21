using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Text;
using PopToRelatedFile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PopToRelatedFile
{
    public class CshtmlLinkedJsRelatedFileDetector : IRelatedFileDetector
    {
        IDocumentService documentService;

        string scriptPattern = @"<script[^>]*\s+src=['""]([^'""]+)['""][^>]*>";

        public CshtmlLinkedJsRelatedFileDetector(IDocumentService documentService)
        {
            this.documentService = documentService;
        }

        public Task<List<string>> CorrespondingFiles(ITextDocument document)
        {
            var urls = this.ScriptUrls(document);

            return Task.FromResult(urls);
        }

        public List<string> ScriptUrls(ITextDocument document)
        {
            var text = this.documentService.GetDocumentText(document);

            var matches = Regex.Matches(text, scriptPattern, RegexOptions.IgnoreCase);
            var urls = new List<string>();

            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    urls.Add(match.Groups[1].Value);
                }
            }

            return urls;
        }

        public string ProjectUrl(string url)
        {
            //in progress
            //VS.Solutions.GetCurrentSolution().FindAncestor( a => a.Name)
        }

        public async Task<bool> IsType(ITextDocument document)
        {
            var path = await documentService.FilePath(document);
            return path.EndsWith(".cshtml");
        }
    }
}
