using Connect4Engine.Core.Operation;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Connect4Engine.UI.WPF.Converters.Game
{
    public sealed class ColourToBrushConverter : IValueConverter
    {
        private static readonly SolidColorBrush Red = new(Color.FromRgb(227, 52, 52));
        private static readonly SolidColorBrush Yellow = new(Color.FromRgb(250, 224, 80));
        private static readonly SolidColorBrush None = new(Color.FromArgb(64, 255, 255, 255));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Colour colour
                ? (object)(colour switch
                {
                    Colour.Red => Red,
                    Colour.Yellow => Yellow,
                    _ => None,
                })
                : throw new NotSupportedException($"{value} is not a supported {nameof(Colour)}.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
