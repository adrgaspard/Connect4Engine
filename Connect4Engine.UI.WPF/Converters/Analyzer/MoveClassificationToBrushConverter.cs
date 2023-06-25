using Connect4Engine.Core.AI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Connect4Engine.UI.WPF.Converters.Analyzer
{
    public sealed class MoveClassificationToBrushConverter : IValueConverter
    {
        private static readonly SolidColorBrush Brilliant = new(Color.FromRgb(0x1B, 0xAD, 0xA6));
        private static readonly SolidColorBrush Great = new(Color.FromRgb(0x5C, 0x8B, 0xB0));
        private static readonly SolidColorBrush Best = new (Color.FromRgb(0x96, 0xBC, 0x4B));
        private static readonly SolidColorBrush Excellent = new (Color.FromRgb(0x96, 0xBC, 0x4B));
        private static readonly SolidColorBrush Good = new (Color.FromRgb(0x96, 0xAF, 0x8B));
        private static readonly SolidColorBrush Theoric = new(Color.FromRgb(0xA8, 0x88, 0x65));
        private static readonly SolidColorBrush MissedWin = new (Color.FromRgb(0xDB, 0xAC, 0x16));
        private static readonly SolidColorBrush Inaccuracy = new (Color.FromRgb(0xF7, 0xC0, 0x45));
        private static readonly SolidColorBrush Mistake = new (Color.FromRgb(0xE5, 0x8F, 0x2A));
        private static readonly SolidColorBrush Blunder = new (Color.FromRgb(0xCA, 0x34, 0x31));
        private static readonly SolidColorBrush OnlyOne = new (Color.FromRgb(0xB8, 0xB8, 0xC2));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (MoveClassification)value switch
            {
                MoveClassification.Brilliant => Brilliant,
                MoveClassification.Great => Great,
                MoveClassification.Best => Best,
                MoveClassification.Excellent => Excellent,
                MoveClassification.Good => Good,
                MoveClassification.Theoric => Theoric,
                MoveClassification.MissedWin => MissedWin,
                MoveClassification.Inaccuracy => Inaccuracy,
                MoveClassification.Mistake => Mistake,
                MoveClassification.Blunder => Blunder,
                MoveClassification.OnlyOne => OnlyOne,
                _ => throw new NotSupportedException(),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
