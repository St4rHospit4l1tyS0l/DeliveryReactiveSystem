using System;
using System.Windows.Data;
using System.Windows.Media;
using Drs.Model.Settings;

namespace Drs.Ui.Converter
{
    public class EnumToVisualConverter : IValueConverter
    {

        public Visual AppbarMono { get; set; }
        public Visual AppbarWebcam { get; set; }
        public Visual AppbarMarvelIronman { get; set; }
        public Visual AppbarMarvelCaptainamerica { get; set; }
        public Visual AppbarMarvelAvengers { get; set; }
        public Visual AppbarStarwarsRebel { get; set; }
        public Visual AppbarStarwarsJedi { get; set; }
        public Visual AppbarStarwarsSith { get; set; }



        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is string))
                return null; //Or a some default Visual

            var val = (string)value;

            switch (val)
            {
                case "appbar_mono":
                    return AppbarMono;
                case "appbar_webcam":
                    return AppbarWebcam;
                default:
                    return AppbarMono;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
