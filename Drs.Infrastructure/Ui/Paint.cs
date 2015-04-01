using System;
using System.Windows.Media;

namespace Drs.Infrastructure.Ui
{
    public static class Paint
    {
        public static Color ToRgbColor(this string color)
        {
            try
            {
                return Color.FromRgb(Convert.ToByte(color.Substring(1, 2), 16),
                    Convert.ToByte(color.Substring(3, 2), 16),
                    Convert.ToByte(color.Substring(5, 2), 16));
            }
            catch (Exception)
            {
                return Color.FromRgb(0, 0, 0);
            }
        }

        public static Color ToRgbLightColor(this string color, int add)
        {
            try
            {
                return Color.FromRgb(Convert.ToByte(color.Substring(1, 2), 16).AddValueWithMax(add),
                    Convert.ToByte(color.Substring(3, 2), 16).AddValueWithMax(add),
                    Convert.ToByte(color.Substring(5, 2), 16).AddValueWithMax(add));
            }
            catch (Exception)
            {
                return Color.FromRgb(0, 0, 0);
            }
        }

        public static byte AddValueWithMax(this byte value, int add)
        {
            if (value + add > 255)
                return 255;


            if (value + add < 0)
                return 0;

            return (byte) (value + add);
        }
    }
}
