using Autoharp.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoharp.Services
{
    public class VsSolutionService : IVsSolutionService
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
            await this.JumpulateFileAsync(file);
            if (file.TextDocument == null)
            {
                return false;
            }
            return file.TextDocument.TextBuffer.ContentType.IsOfType(type);
        }

        public async Task<string> GetDocumentTextAsync(File file)
        {
            await this.JumpulateFileAsync(file);
            return file.TextDocument?.TextBuffer.CurrentSnapshot.GetText();
        }

        public File GetFileForDocumentView(DocumentView documentView) =>
            new File(documentView?.Document.FilePath);

        private async Task JumpulateFileAsync(File file)
        {
            if (file.TextDocument == null)
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

            var solution = await VS.Solutions.GetCurrentSolutionAsync();
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

        public IEnumerable<Token> GetClasses(File file)
        {
            this.JumpulateFileAsync(file); 

            var tree = CSharpSyntaxTree.ParseText(file.TextDocument.TextBuffer.CurrentSnapshot.GetText());
            var root = tree.GetCompilationUnitRoot();

            var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            return classDeclarations.Select(cd => new Token(cd));
        }

        public IEnumerable<Token> GetClassAncestors(Token classToken)
        {
            var classNode = classToken.node as ClassDeclarationSyntax;
            return classNode?.BaseList?.Types.Select(t => new Token(t));
        }

        public bool IsTokenUserDefined(Token c)
        {
            throw new NotImplementedException();
        }

        public File GetFile(Token c)
        {
            throw new NotImplementedException();
        }
    }
}
