namespace Drs.Ui.Ui.Shared
{
    /// <summary>
    /// Interaction logic for AutoCompleteSecPhoneUc.xaml
    /// </summary>
    public partial class AutoCompleteSecPhoneUc
    {
        public AutoCompleteSecPhoneUc()
        {
            InitializeComponent();
            var autoComplete = new AutoCompleteTextUcBase(SearchTxtBox, ListData, PopupList, IsDone, false);
        }

        /*
        public static DependencyProperty IsControlFocusedProperty = DependencyProperty.Register("IsControlFocused", typeof(bool), typeof(AutoCompleteTextUc));


        public static bool GetIsControlFocused(UIElement element)
        {
            return (bool)element.GetValue(IsControlFocusedProperty);
        }

        public static void SetIsControlFocused(UIElement element, bool value)
        {
            element.SetValue(IsControlFocusedProperty, value);
        }

        public bool IsControlFocused
        {
            get
            {
                return (bool)GetValue(IsControlFocusedProperty);
            }
            set { SetValue(IsControlFocusedProperty, value); }
        }*/
 
    }
}
