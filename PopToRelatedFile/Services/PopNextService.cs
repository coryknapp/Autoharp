using Community.VisualStudio.Toolkit.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PopToRelatedFile.Services
{
    public class PopNextService : IPopNextService
    {
        private List<IRelatedFileDetector> relatedFileDetectors;

        private List<string> RelatedFileList { get; set; }

        private string MostRecentlyPoppedToFilePath { get; set; }

        private string OriginFilePath { get; set; }

        DIToolkitPackage package;

        public PopNextService(DIToolkitPackage package)
        {
            this.InitializeFileDetectors(package);
        }

        private void InitializeFileDetectors(DIToolkitPackage package)
        {
            this.relatedFileDetectors = new List<IRelatedFileDetector>()
            {
                package.ServiceProvider.GetService<CsRelatedFileDetector>(),
                package.ServiceProvider.GetService<CshtmlRelatedFileDetector>(),
                //package.ServiceProvider.GetService<CshtmlLinkedJsRelatedFileDetector>(),
            };
        }

        public async Task GoToNextFile()
        {
            if (this.RelatedFileList == null)
            {
                await this.ResetOrigin();
            }

            await this.OpenNextFile();
        }

        public async Task ResetOrigin()
        {
            var documentView = await VS.Documents.GetActiveDocumentViewAsync();
            var filePath = documentView?.Document.FilePath;

            this.RelatedFileList = await this.GetRelatedFiles(filePath);
            this.OriginFilePath = filePath;
            await VS.StatusBar.ShowMessageAsync($"PopToRelatedFile origin: '{Path.GetFileName(filePath)}'.  Found {this.RelatedFileList.Count} files.");
        }

        private async Task<List<string>> GetRelatedFiles(string filePath)
        {
            var documentView = await VS.Documents.GetActiveDocumentViewAsync();

            var relatedFiles = new List<string>() { documentView?.Document.FilePath };
            foreach (var rfd in relatedFileDetectors)
            {
                if (await rfd.IsType(documentView?.Document))
                {
                    relatedFiles.AddRange(await rfd.CorrespondingFiles(documentView?.Document));
                }
            }

            this.MostRecentlyPoppedToFilePath = relatedFiles.First();

            return relatedFiles;
        }

        private async Task OpenNextFile()
        {
            var nextPath = this.NextPath();
            await VS.Documents.OpenAsync(nextPath);
            this.MostRecentlyPoppedToFilePath = nextPath;
        }

        private string NextPath()
        {
            var currentIndex = RelatedFileList.IndexOf(MostRecentlyPoppedToFilePath);
            var nextIndex = (currentIndex + 1) % this.RelatedFileList.Count;
            return this.RelatedFileList[nextIndex];
        }
    }
}
