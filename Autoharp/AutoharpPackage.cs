global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;
using Community.VisualStudio.Toolkit.DependencyInjection.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using Autoharp.Services;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Autoharp
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.AutoharpString)]
    public sealed class AutoharpPackage : MicrosoftDIToolkitPackage<AutoharpPackage>
    {
        protected override void InitializeServices(IServiceCollection services)
        {

            // Register your services here
            services.AddSingleton<IVsSolutionService, VsSolutionService>();
            services.AddSingleton<IJumpNextService, JumpNextService>();
            services.AddSingleton<IInformationService, InformationService>();

            services.AddSingleton<CsToCshtmlFileDetector>();
            services.AddSingleton<CshtmlToCsFileDetector>();
            services.AddSingleton<CsClassAncestorsDetector>();
            services.AddSingleton<CshtmlLinkedJsRelatedFileDetector>();

            base.InitializeServices(services);
            services.RegisterCommands(ServiceLifetime.Singleton);
        }

        //protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        //{
        //    await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
        //    await base.InitializeAsync(cancellationToken, progress);
        //}
    }
}