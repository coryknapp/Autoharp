using Community.VisualStudio.Toolkit.DependencyInjection;
using Community.VisualStudio.Toolkit.DependencyInjection.Core;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using PopToRelatedFile.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PopToRelatedFile
{
    [Command(PackageIds.PopNextAndResetCommand)]
    public class PopNextAndResetCommand : BaseDICommand
    {
        private readonly IPopNextService popNextService;

        public PopNextAndResetCommand(DIToolkitPackage package, IPopNextService popNextService): base(package)
        {
            this.popNextService = popNextService;
        }

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await this.popNextService.ResetOriginAsync();
            await this.popNextService.GoToNextFileAsync();
        }
    }
}
