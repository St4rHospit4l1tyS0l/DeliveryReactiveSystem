using System;
using MahApps.Metro.Controls.Dialogs;

namespace Drs.Model.Shared
{
    public class MessageBoxSettings
    {
        public MessageBoxSettings()
        {
            Settings = new MetroDialogSettings();
        }
        public MetroDialogSettings Settings { get; set; }
        public MessageDialogStyle Style { get; set; }
        public String Title { get; set; }
        public String Message { get; set; }
        
        public Action<MessageDialogResult> Callback;
    }
}
