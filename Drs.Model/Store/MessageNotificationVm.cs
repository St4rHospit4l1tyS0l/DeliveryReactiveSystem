using System.Windows;
using System.Windows.Media;
using Drs.Model.Constants;
using ReactiveUI;

namespace Drs.Model.Store
{
    public class MessageNotificationVm : ReactiveObject
    {
        private string _message;
        private ImageSource _itemImage;

        public string Message
        {
            get
            {
                return _message.StartsWith(SharedConstants.NO_MESSAGE) ? string.Empty : _message;
            }
            set { _message = value; }
        }

        public ImageSource ItemImage
        {
            get
            {
                return _itemImage;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _itemImage, value);
            }
        }

        public Visibility HasToShowResource {
            get
            {
                return _itemImage == null ? Visibility.Collapsed : Visibility.Visible;
            }
        }
    }
}