using Connect4Engine.Core.AI;
using MahApps.Metro.IconPacks;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Connect4Engine.UI.WPF.Converters.Analyzer
{
    public sealed class MoveClassificationToFAKindConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (MoveClassification)value switch
            {
                MoveClassification.Brilliant => PackIconFontAwesomeKind.CrownSolid,
                MoveClassification.Great => PackIconFontAwesomeKind.ExclamationSolid,
                MoveClassification.Best => PackIconFontAwesomeKind.StarSolid,
                MoveClassification.Excellent => PackIconFontAwesomeKind.ThumbsUpSolid,
                MoveClassification.Good => PackIconFontAwesomeKind.CheckSolid,
                MoveClassification.Theoric => PackIconFontAwesomeKind.BookSolid,
                MoveClassification.MissedWin => PackIconFontAwesomeKind.MinusSolid,
                MoveClassification.Inaccuracy => PackIconFontAwesomeKind.CrosshairsSolid,
                MoveClassification.Mistake => PackIconFontAwesomeKind.QuestionSolid,
                MoveClassification.Blunder => PackIconFontAwesomeKind.TimesSolid,
                MoveClassification.OnlyOne => PackIconFontAwesomeKind.DotCircleSolid,
                _ => throw new NotSupportedException(),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
