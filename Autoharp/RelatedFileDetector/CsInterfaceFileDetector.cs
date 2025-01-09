using Autoharp.Models;
using Autoharp.Services;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoharp
{
    public class CsInterfaceFileDetector : IRelatedFileDetector
    {
        IDocumentService documentService;

        public CsInterfaceFileDetector(IDocumentService documentService)
        {
            this.documentService = documentService;
        }

        public async Task<IEnumerable<File>> CorrespondingFilesAsync(File document)
        {
            var fileContent = await this.documentService.GetDocumentTextAsync(document);

            var syntaxTree = CSharpSyntaxTree.ParseText(fileContent);

            var root = await syntaxTree.GetRootAsync();
            //var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            //foreach (var classDecl in classDeclarations)
            //{
            //    Console.WriteLine($"Class: {classDecl.Identifier.Text}");

            //    // Retrieve the implemented interfaces
            //    var interfaces = classDecl.BaseList?.Types
            //        .Where(baseType => baseType.Type is SimpleNameSyntax)
            //        .Select(baseType => baseType.ToString());

            //    if (interfaces != null && interfaces.Any())
            //    {
            //        Console.WriteLine("Implements Interfaces:");
            //        foreach (var iface in interfaces)
            //        {
            //            Console.WriteLine($"  - {iface}");
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine("No implemented interfaces.");
            //    }
            //}

            return new File[] {};
        }

        public async Task<bool> IsTypeAsync(File file) =>
            await documentService.IsTypeAsync(file, "CSharp")
                && !file.FullPath.EndsWith(".cshtml");
    }
}
