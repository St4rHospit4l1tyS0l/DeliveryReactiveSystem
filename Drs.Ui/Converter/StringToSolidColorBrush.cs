using System;
using System.Windows.Data;
using System.Windows.Media;
using Drs.Infrastructure.Ui;

namespace Drs.Ui.Converter
{

    [ValueConversion(typeof(string), typeof(SolidColorBrush))]
    public class StringToSolidColorBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return new SolidColorBrush(((string)value).ToRgbColor());
            }
            catch (Exception)
            {
                return new SolidColorBrush(Colors.Black);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
