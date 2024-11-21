global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;
using Community.VisualStudio.Toolkit.DependencyInjection.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using PopToRelatedFile.Services;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace PopToRelatedFile
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.PopToRelatedFileString)]
    public sealed class PopToRelatedFilePackage : MicrosoftDIToolkitPackage<PopToRelatedFilePackage>
    {
        protected override void InitializeServices(IServiceCollection services)
        {
            base.InitializeServices(services);

            // Register your services here
            services.AddSingleton<IDocumentService, DocumentService>();
            services.AddSingleton<IPopNextService, PopNextService>();

            services.AddSingleton<CsRelatedFileDetector>();
            services.AddSingleton<CshtmlRelatedFileDetector>();
            //services.AddSingleton<CshtmlLinkedJsRelatedFileDetector>();

            services.AddSingleton<PopNextAndResetCommand>();
            services.AddSingleton<PopNextCommand>();

            // Automatically register all commands in an assembly.
            //services.RegisterCommands(ServiceLifetime.Singleton);
        }

        //protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        //{
        //    await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
        //    await base.InitializeAsync(cancellationToken, progress);
        //}
    }
}