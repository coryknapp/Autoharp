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
    [Command(PackageIds.JumpNextCommand)]
    public sealed class JumpNextCommand : BaseDICommand
    {
        private readonly IJumpNextService popNextService;

        public JumpNextCommand(DIToolkitPackage package, IJumpNextService popNextService) : base(package)
        {
            this.popNextService = popNextService;
        }

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await this.popNextService.GoToNextFileAsync();
        }

    }
}
