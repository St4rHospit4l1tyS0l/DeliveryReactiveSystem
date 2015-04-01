using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Drs.Model.Constants;
using Drs.Model.Shared;

namespace Drs.Ui.Ui.Shared
{
    internal class AutoCompleteTextUcBase
    {
        private readonly TextBox _searchTxtBox;
        private readonly ListBox _listData;
        private readonly Popup _popupList;
        private readonly Button _isDone;
        private bool _bHasToFocus;


        public AutoCompleteTextUcBase(TextBox searchTxtBox, ListBox listData, Popup popupList, Button isDone, bool bHasToFocus = true)
        {
            _searchTxtBox = searchTxtBox;
            _listData = listData;
            _popupList = popupList;
            _isDone = isDone;
            _bHasToFocus = bHasToFocus;
        
            _searchTxtBox.KeyUp += SearchTxtBox_OnKeyUp;
            if (_bHasToFocus)
                _searchTxtBox.IsVisibleChanged += SearchTxtBox_IsVisibleChanged;
            _listData.MouseLeftButtonUp += ListBox_MouseLeftButtonUp;
            _searchTxtBox.LostFocus += SearchTxtBox_OnLostFocus;
            ((INotifyCollectionChanged)_listData.Items).CollectionChanged += ListBox_CollectionChanged;
        }

        private void SearchTxtBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (_isDone.Content != null && _isDone.Content.ToString() != SharedConstants.Client.IS_TRUE && _listData.IsMouseOver == false)
                SetTextAndHide();
        }

        public void ListBox_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_listData.Items == null || _listData.Items.Count == 0 || e.NewItems == null)
            {
                _popupList.IsOpen = false;
                return;
            }

            if (_popupList.IsOpen == false)
                _popupList.IsOpen = true;

        }

        public void SearchTxtBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (_listData.SelectedIndex > 0)
                {
                    _listData.SelectedIndex--;
                }
                return;
            }
            if (e.Key == Key.Down)
            {
                if (_listData.SelectedIndex < _listData.Items.Count - 1)
                {
                    _listData.SelectedIndex++;
                }
                return;
            }
            if (e.Key == Key.Enter)
            {
                //if (PopupList.IsOpen && _listData.Items.Count > 0)
                SetTextAndHide();
                //else if (this.SearchText.Length < _parialSearchTextLength)
                //    this.TextBoxEnterCommand.Execute(null);
                return;
            }

            _isDone.Content = SharedConstants.Client.IS_FALSE;

        }

        public void SetTextAndHide()
        {
            _popupList.IsOpen = false;
            _isDone.Content = SharedConstants.Client.IS_TRUE;

            if (_listData.SelectedItem != null)
            {
                _searchTxtBox.Text = ((ListItemModel)_listData.SelectedItem).Value;
            }

            var dataSel = _listData.SelectedItem ?? new ListItemModel { Key = String.Empty, Value = _searchTxtBox.Text };

            _isDone.Command.Execute(dataSel);
            _searchTxtBox.Focus();
            _searchTxtBox.SelectAll();
        }

        public void SearchTxtBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _searchTxtBox.Focus();
        }

        public void ListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetTextAndHide();
        }

        public void EnableFocus()
        {
            if (_bHasToFocus)
                return;

            _bHasToFocus = false;

            _searchTxtBox.IsVisibleChanged += SearchTxtBox_IsVisibleChanged;
        }
    }
}