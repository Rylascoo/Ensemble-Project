using Microsoft.UI.Xaml.Data;

namespace Kymaean.Windows.Presentation;

public sealed class HistoryCountToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is int count
            ? $"{count} recorded events in this local fixture"
            : string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
