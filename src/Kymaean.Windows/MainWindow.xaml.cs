using Kymaean.Application;
using Microsoft.UI.Xaml;

namespace Kymaean.Windows;

public sealed partial class MainWindow : Window
{
    public MainWindow(WorkspaceApplication workspace)
    {
        ArgumentNullException.ThrowIfNull(workspace);
        InitializeComponent();

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        AppWindow.SetIcon("Assets/AppIcon.ico");

        RootFrame.Navigate(typeof(MainPage), workspace);
    }
}
