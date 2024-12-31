using Microsoft.VisualStudio.Text;
using PopToRelatedFile.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PopToRelatedFile.Models;

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

        public async Task<IEnumerable<File>> CorrespondingFilesAsync(File document)
        {
            var urls = await this.GetScriptUrlsAsync(document);
            var filteredUrls = await this.FilterUrlsAsync(urls);
            return filteredUrls.Select(u => new File(u)).ToList();
        }

        public async Task<List<string>> GetScriptUrlsAsync(File document)
        {
            var text = await this.documentService.GetDocumentTextAsync(document);

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

        public async Task<IEnumerable<string>> FilterUrlsAsync(IEnumerable<string> scriptUrls)
        {
            var localUrls = scriptUrls.Where(this.IsUrlLocal);
            var scriptFileNames = scriptUrls.Select(System.IO.Path.GetFileName);
            var projectFiles = await this.documentService.GetAllFilesAsync(this.MakeFilter(scriptFileNames));

            return projectFiles.Select(f => f.FullPath);
        }

        public bool IsUrlLocal(string url) =>
            url.StartsWith("~/");

        private Func<File, bool> MakeFilter(IEnumerable<string> fileNames) =>
            new Func<File, bool>(item => fileNames.Contains(System.IO.Path.GetFileName(item.FullPath)));

        public async Task<bool> IsTypeAsync(File file) =>
            file.FullPath.EndsWith(".cshtml");
    }
}
