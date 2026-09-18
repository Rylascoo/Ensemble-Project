using Kymaean.Application;
using Kymaean.Infrastructure.Persistence;
using Microsoft.UI.Xaml;

namespace Kymaean.Windows;

public partial class App : Microsoft.UI.Xaml.Application
{
    private Window? _window;

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
                var catalog = new FileProductionCatalog(applicationRoot);
                return ProductApplication.Start(catalog);
            });

        _window = new MainWindow(startup);
        _window.Activate();
    }
}
