using Kymaean.Application;
using Kymaean.Infrastructure.Persistence;
using Microsoft.UI.Xaml;

namespace Kymaean.Windows;

public partial class App : Microsoft.UI.Xaml.Application
{
    private Window? _window;
#if KYMAEAN_DIRECTOR_PREVIEW
    private IDisposable? _previewLease;
#endif

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var startup = WindowsStartupComposition.Compose(
            () =>
            {
                var applicationRoot =
                    global::Windows.Storage.ApplicationData.Current.LocalFolder.Path;
#if KYMAEAN_DIRECTOR_PREVIEW
                _previewLease = Preview.PreviewEnvironment.Acquire(applicationRoot);
                applicationRoot = Preview.PreviewEnvironment.SelectedDataRoot(applicationRoot);
#endif
                var catalog = new FileProductionCatalog(applicationRoot);
                return ProductApplication.Start(catalog);
            });

        _window = new MainWindow(startup);
        _window.Activate();
#if KYMAEAN_DIRECTOR_PREVIEW
        _window.Closed += (_, _) => _previewLease?.Dispose();
        Preview.PreviewEnvironment.RecordLaunch(
            global::Windows.Storage.ApplicationData.Current.LocalFolder.Path,
            !startup.IsInfrastructureFailure && startup.ProductStartup.IsSuccess);
#endif
    }
}
