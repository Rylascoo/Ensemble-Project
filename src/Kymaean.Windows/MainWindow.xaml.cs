using System.Runtime.InteropServices;
using Kymaean.Application;
using Microsoft.UI.Xaml;

namespace Kymaean.Windows;

public sealed partial class MainWindow : Window
{
    private const int GwlWndProc = -4;
    private const uint WmGetMinMaxInfo = 0x0024;
    private const int MinimumWindowWidth = 720;
    private const int MinimumWindowHeight = 520;

    private readonly WindowProcedure _windowProcedure;
    private readonly IntPtr _windowHandle;
    private readonly IntPtr _originalWindowProcedure;

    public MainWindow(WorkspaceApplication workspace)
    {
        ArgumentNullException.ThrowIfNull(workspace);
        InitializeComponent();

        _windowProcedure = HandleWindowMessage;
        _windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(this);
        var procedurePointer = Marshal.GetFunctionPointerForDelegate(_windowProcedure);
        _originalWindowProcedure = SetWindowLongPtr(_windowHandle, GwlWndProc, procedurePointer);
        if (_originalWindowProcedure == IntPtr.Zero)
        {
            throw new InvalidOperationException("Could not install the native minimum-window contract.");
        }

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        AppWindow.SetIcon("Assets/AppIcon.ico");

        RootFrame.Navigate(typeof(MainPage), workspace);
    }

    private IntPtr HandleWindowMessage(
        IntPtr windowHandle,
        uint message,
        IntPtr wParam,
        IntPtr lParam)
    {
        if (message == WmGetMinMaxInfo)
        {
            var info = Marshal.PtrToStructure<MinMaxInfo>(lParam);
            info.MinTrackSize.X = MinimumWindowWidth;
            info.MinTrackSize.Y = MinimumWindowHeight;
            Marshal.StructureToPtr(info, lParam, false);
        }

        return CallWindowProc(
            _originalWindowProcedure,
            windowHandle,
            message,
            wParam,
            lParam);
    }

    private delegate IntPtr WindowProcedure(
        IntPtr windowHandle,
        uint message,
        IntPtr wParam,
        IntPtr lParam);

    [StructLayout(LayoutKind.Sequential)]
    private struct Point
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MinMaxInfo
    {
        public Point Reserved;
        public Point MaxSize;
        public Point MaxPosition;
        public Point MinTrackSize;
        public Point MaxTrackSize;
    }

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
    private static extern IntPtr SetWindowLongPtr(
        IntPtr windowHandle,
        int index,
        IntPtr newProcedure);

    [DllImport("user32.dll", EntryPoint = "CallWindowProcW")]
    private static extern IntPtr CallWindowProc(
        IntPtr previousProcedure,
        IntPtr windowHandle,
        uint message,
        IntPtr wParam,
        IntPtr lParam);
}
