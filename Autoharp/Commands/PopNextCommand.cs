using Community.VisualStudio.Toolkit.DependencyInjection;
using Community.VisualStudio.Toolkit.DependencyInjection.Core;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Autoharp.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Autoharp
{
    [Command(PackageIds.PopNextCommand)]
    public sealed class PopNextCommand : BaseDICommand
    {
        private readonly IPopNextService popNextService;

        public PopNextCommand(DIToolkitPackage package, IPopNextService popNextService) : base(package)
        {
            this.popNextService = popNextService;
        }

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await this.popNextService.GoToNextFileAsync();
        }

    }
}
