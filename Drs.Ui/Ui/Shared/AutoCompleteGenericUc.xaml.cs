using Drs.ViewModel.Shared;

namespace Drs.Ui.Ui.Shared
{
    /// <summary>
    /// Interaction logic for AutoCompleteGenericUc.xaml
    /// </summary>
    public partial class AutoCompleteGenericUc
    {
        private readonly AutoCompleteTextUcBase _autoHelper;

        public AutoCompleteGenericUc()
        {
            InitializeComponent();
            _autoHelper = new AutoCompleteTextUcBase(SearchTxtBox, ListData, PopupList, IsDone, false);
        }

        private void AutoCmp_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var dataContext = (IAutoCompleteTextVm) DataContext;
            if (dataContext.IsFocused)
                _autoHelper.EnableFocus();

        }
    }
}
