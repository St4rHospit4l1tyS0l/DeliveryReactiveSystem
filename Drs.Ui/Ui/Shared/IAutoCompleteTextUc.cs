using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Drs.Ui.Ui.Shared
{
    public interface IAutoCompleteTextUc
    {
        ListBox ListDataEx { get;}
        Popup PopupListEx { get; }
        Button IsDoneEx { get; }
        TextBox SearchTxtBoxEx { get; }
    }
}