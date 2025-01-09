using Community.VisualStudio.Toolkit.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Autoharp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autoharp.Services
{
    public class PopNextService : IPopNextService
    {
        private IDocumentService documentService { get; set; }

        private IInformationService informationService { get; set; }

        private List<IRelatedFileDetector> relatedFileDetectors;

        private List<File> RelatedFileList { get; set; }

        private string MostRecentlyPoppedToFilePath { get; set; }

        private string OriginFilePath { get; set; }

        public PopNextService(DIToolkitPackage package, IDocumentService documentService, IInformationService informationService)
        {
            this.documentService = documentService;
            this.informationService = informationService;
            this.InitializeFileDetectors(package);
        }

        private void InitializeFileDetectors(DIToolkitPackage package)
        {
            this.relatedFileDetectors = new List<IRelatedFileDetector>()
            {
                package.ServiceProvider.GetService<CsToCshtmlFileDetector>(),
                package.ServiceProvider.GetService<CshtmlToCsFileDetector>(),
                package.ServiceProvider.GetService<CsInterfaceFileDetector>(),
                package.ServiceProvider.GetService<CshtmlLinkedJsRelatedFileDetector>(),
            };
        }

        public async Task GoToNextFileAsync()
        {
            if (this.RelatedFileList == null)
            {
                await this.ResetOriginAsync();
            }

            await this.OpenNextFileAsync();
        }

        public async Task ResetOriginAsync()
        {
            var documentView = await VS.Documents.GetActiveDocumentViewAsync();
            var filePath = documentView?.Document.FilePath;

            var file = documentService.GetFileForDocumentView(documentView);

            this.RelatedFileList = await this.GetRelatedFilesAsync(file);
            this.OriginFilePath = filePath;
            await informationService.InformResetOriginAsync(this.OriginFilePath, this.RelatedFileList);
        }

        private async Task<List<File>> GetRelatedFilesAsync(File file)
        {
            var currentFile = await documentService.GetCurrentFileAsync();
            var relatedFiles = new List<File>() { currentFile };
            foreach (var detector in relatedFileDetectors)
            {
                if (await detector.IsTypeAsync(currentFile))
                {
                    try
                    {
                        relatedFiles.AddRange(await detector.CorrespondingFilesAsync(currentFile));
                    }
                    catch(Exception ex)
                    {
                        await informationService.LogErrorAsync(this.FormatErrorMessage(detector, ex));
                    }
                }
            }

            this.MostRecentlyPoppedToFilePath = relatedFiles.First().FullPath;

            return relatedFiles;
        }

        private async Task OpenNextFileAsync()
        {
            var nextPath = this.NextPath();
            await VS.Documents.OpenAsync(nextPath);
            this.MostRecentlyPoppedToFilePath = nextPath;
        }

        private string NextPath()
        {
            var currentIndex = RelatedFileList.IndexOf(new File(MostRecentlyPoppedToFilePath));
            var nextIndex = (currentIndex + 1) % this.RelatedFileList.Count;
            return this.RelatedFileList[nextIndex].FullPath;
        }

        private string FormatErrorMessage(IRelatedFileDetector detector, Exception ex) =>
            $@"Error in ${detector.GetType().Name}: {ex.Message}";
    }
}
