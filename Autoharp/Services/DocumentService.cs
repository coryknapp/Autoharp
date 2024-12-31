using Community.VisualStudio.Toolkit.DependencyInjection;
using Microsoft.VisualStudio.Text;
using Autoharp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autoharp.Services
{
    public class DocumentService : IDocumentService
    {
        public bool FileExists(File file) =>
            System.IO.File.Exists(file.FullPath);

        public async Task<File> GetCurrentFileAsync()
        {
            var documentView = await VS.Documents.GetActiveDocumentViewAsync();
            var path = documentView?.Document?.FilePath;
            if (path == null)
            {
                return null;
            }
            return new File(path);
        }

        public async Task<bool> IsTypeAsync(File file, string type)
        {
            await this.PopulateFileAsync(file);
            if (file.TextDocument == null)
            {
                return false;
            }
            return file.TextDocument.TextBuffer.ContentType.IsOfType(type);
        }

        public async Task<string> GetDocumentTextAsync(File file)
        {
            await this.PopulateFileAsync(file);
            return file.TextDocument?.TextBuffer.CurrentSnapshot.GetText();
        }

        public File GetFileForDocumentView(DocumentView documentView) =>
            new File(documentView?.Document.FilePath);

        private async Task PopulateFileAsync(File file)
        {
            if (file.TextDocument  == null)
            {
                var documentView = await VS.Documents.GetDocumentViewAsync(file.FullPath);
                if (documentView != null)
                {
                    file.TextDocument = documentView.Document;
                }
            }
        }

        public async Task<IEnumerable<File>> GetAllFilesAsync(Func<File, bool> filter = null)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            Solution solution = await VS.Solutions.GetCurrentSolutionAsync();
            List<SolutionItem> list = new List<SolutionItem>();

            this.AddItemsToList(list, solution, filter);

            return list.Select(si => new File(si.FullPath));
        }

        private void AddItemsToList(List<SolutionItem> list, SolutionItem solutionItem, Func<File, bool> filter)
        {
            if (filter == null || filter(new File(solutionItem.FullPath)))
            {
                list.Add(solutionItem);
            }

            foreach (SolutionItem item in solutionItem.Children)
            {
                AddItemsToList(list, item, filter);
            }
        }
    }
}
