using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Drs.Infrastructure.Ui
{
    public class GridBehaviours
    {
        public static DependencyProperty DoubleClickCommandProperty =
           DependencyProperty.RegisterAttached("DoubleClickCommand", typeof(ICommand), typeof(GridBehaviours),
                                               new PropertyMetadata(DoubleClick_PropertyChanged));

        public static void SetDoubleClickCommand(UIElement element, ICommand value)
        {
            element.SetValue(DoubleClickCommandProperty, value);
        }

        public static ICommand GetDoubleClickCommand(UIElement element)
        {
            return (ICommand)element.GetValue(DoubleClickCommandProperty);
        }

        private static void DoubleClick_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var row = d as DataGridRow;
            if (row == null) return;

            if (e.NewValue != null)
            {
                row.AddHandler(Control.MouseDoubleClickEvent, new RoutedEventHandler(DataGrid_MouseDoubleClick));
            }
            else
            {
                row.RemoveHandler(Control.MouseDoubleClickEvent, new RoutedEventHandler(DataGrid_MouseDoubleClick));
            }
        }

        private static void DataGrid_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var row = sender as DataGridRow;

            if (row != null)
            {
                var cmd = GetDoubleClickCommand(row);
                if (cmd.CanExecute(row.Item))
                    cmd.Execute(row.Item);
            }
        }
    }
}
