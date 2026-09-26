#if KYMAEAN_DIRECTOR_PREVIEW
using System.Text.Json;
using Microsoft.UI.Xaml;

namespace Kymaean.Windows.Preview;

// External validation apparatus only; never a Product setting or shipping type.
internal static class PreviewView
{
    internal static void Apply(Window window)
    {
        var data = PreviewEnvironment.SelectedDataRoot(global::Windows.Storage.ApplicationData.Current.LocalFolder.Path);
        var file = PreviewEnvironment.SafePath(Path.GetDirectoryName(data)!, "view.json");
        if (!File.Exists(file)) return;
        using var document = JsonDocument.Parse(File.ReadAllText(file));
        var view = document.RootElement;
        var width = view.GetProperty("width").GetInt32();
        var height = view.GetProperty("height").GetInt32();
        var theme = view.GetProperty("theme").GetString();
        if ((width, height) is not ((1200, 800) or (720, 520)) || theme is not ("Light" or "Dark"))
            throw new IOException("Unsupported Preview validation view.");
        ((FrameworkElement)window.Content).RequestedTheme = theme == "Light" ? ElementTheme.Light : ElementTheme.Dark;
        window.AppWindow.Resize(new global::Windows.Graphics.SizeInt32(width, height));
    }
}
#endif
