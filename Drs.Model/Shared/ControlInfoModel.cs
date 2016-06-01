using System;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace Drs.Model.Shared
{
    public class ControlInfoModel
    {
        private bool _isEnabled;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                Visibility = _isEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public String Title { get; set; }
        public Visibility Visibility { get; set; }
        public string Name { get; set; }
        public dynamic Validation { get; set; }
    }
}