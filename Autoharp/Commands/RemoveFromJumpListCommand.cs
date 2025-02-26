using Autoharp.Services;
using Community.VisualStudio.Toolkit.DependencyInjection;
using Community.VisualStudio.Toolkit.DependencyInjection.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoharp.Commands
{
    [Command(PackageIds.RemoveFromJumpListCommand)]
    public class RemoveFromJumpListCommand : BaseDICommand
    {
        private IJumpNextService jumpService;

        public RemoveFromJumpListCommand(DIToolkitPackage package, IJumpNextService jumpService) : base(package)
        {
            this.jumpService = jumpService;
        }

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await VS.MessageBox.ShowAsync("Command Executed!", "VSIX Command");
        }

        protected override void BeforeQueryStatus(EventArgs e)
        {
            Command.Enabled = Command.Visible = jumpService.IsActiveFileInJumpListAsync().Result;
        }
    }
}
