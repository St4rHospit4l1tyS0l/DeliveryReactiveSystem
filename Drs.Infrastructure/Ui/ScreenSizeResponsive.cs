using System.Collections.Specialized;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Drs.Infrastructure.Ui
{
    public static class ScreenSizeResponsive
    {
        private const int SCREEN_SIZE_1280 = 1280;

        public static void FixSize(FrameworkElement element)
        {
            if (IsNormalScreenSize()) return;
            element.Width = 1600;
            element.Height = 900;
        }

        public static void FixClientAddressWidth(FrameworkElement element)
        {
            if (IsNormalScreenSize()) return;
            element.Width = 1220;
        }

        private static bool IsNormalScreenSize()
        {
            if ((int)SystemParameters.PrimaryScreenWidth > SCREEN_SIZE_1280) return true;
            return false;
        }

        public static void FixOrderDetailWidth(ColumnDefinitionCollection column)
        {
            if (IsNormalScreenSize()) return;
            column.RemoveAt(column.Count - 1);
            column.Add(new ColumnDefinition { Width = new GridLength(300) });
        }

        public static StringDictionary CalculatePosEnviromentVariables(bool bEnableNotification)
        {
            if (IsNormalScreenSize())
            {
                return new StringDictionary
                {
                    {"AlohaLeft", (SystemParameters.PrimaryScreenWidth*(0.15625) + MoveToLeftIfNotificationIsEnabled(bEnableNotification)).ToString(CultureInfo.InvariantCulture)},
                    {"AlohaXRes", (SystemParameters.PrimaryScreenWidth * (0.52083)).ToString(CultureInfo.InvariantCulture)},
                    {"AlohaTop", (SystemParameters.PrimaryScreenHeight * (0.125)).ToString(CultureInfo.InvariantCulture)},
                    {"AlohaYRes", (SystemParameters.PrimaryScreenHeight * (0.74074)).ToString(CultureInfo.InvariantCulture)}
                };
            }
            return new StringDictionary
            {
                {"AlohaLeft", (SystemParameters.PrimaryScreenWidth*(0.16925) + MoveToLeftIfNotificationIsEnabled(bEnableNotification)).ToString(CultureInfo.InvariantCulture)},
                {"AlohaXRes", (SystemParameters.PrimaryScreenWidth * (0.72583)).ToString(CultureInfo.InvariantCulture)},
                {"AlohaTop", (SystemParameters.PrimaryScreenHeight * (0.248)).ToString(CultureInfo.InvariantCulture)},
                {"AlohaYRes", (SystemParameters.PrimaryScreenHeight * (0.66074)).ToString(CultureInfo.InvariantCulture)}
            };
        }

        private static double MoveToLeftIfNotificationIsEnabled(bool bEnableNotification)
        {
            return (bEnableNotification == false) ? 0 : SystemParameters.PrimaryScreenWidth * (0.1);
        }
    }
}
