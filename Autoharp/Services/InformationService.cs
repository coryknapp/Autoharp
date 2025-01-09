using Autoharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoharp.Services
{
    public class InformationService : IInformationService
    {
        private OutputWindowPane outputWindowPane;

        private async Task<OutputWindowPane> GetOutputWindowPaneAsync()
        {
            this.outputWindowPane ??= await VS.Windows.CreateOutputWindowPaneAsync("Autoharp");
            return this.outputWindowPane;
        }

        public async Task LogErrorAsync(string message)
        {
            var outputWindowPane = await GetOutputWindowPaneAsync();
            await outputWindowPane.WriteAsync(message);
        }

        public async Task InformResetOriginAsync(string originFilePath, IEnumerable<File> relatedFileList)
        {
            var message = $"Autoharp origin: '{System.IO.Path.GetFileName(originFilePath)}'.  Found {relatedFileList.Count()} files.";

            await VS.StatusBar.ShowMessageAsync(message);
            var outputWindowPane = await GetOutputWindowPaneAsync();

            await outputWindowPane.WriteLineAsync(message);
            foreach (var file in relatedFileList)
            {
                await outputWindowPane.WriteLineAsync($"\t{System.IO.Path.GetFileName(file.FullPath)}");
            }
        }
    }
}
