using System.ComponentModel;
using Drs.VirtualPos.Annotations;

namespace Drs.VirtualPos.Infrastructure
{
    public abstract class ViewBaseVp : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
