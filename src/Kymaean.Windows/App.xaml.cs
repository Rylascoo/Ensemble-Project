using Kymaean.Application;
using Kymaean.Infrastructure.Demo;
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
        var fixturePath = Path.Combine(
            AppContext.BaseDirectory,
            "Fixtures",
            "missing-raft-0.1.0.json");

        var store = MissingRaftDemoProductionStore.FromFile(fixturePath);
        var workspace = new WorkspaceApplication(store);

        _window = new MainWindow(workspace);
        _window.Activate();
    }
}
