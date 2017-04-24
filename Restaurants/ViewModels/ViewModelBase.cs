using GalaSoft.MvvmLight.Messaging;
using Restaurants.Core.Services;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Restaurants.Registry;

namespace Restaurants.ViewModels
{
    class ViewModelBase : INotifyPropertyChanged
    {
        //basic ViewModelBase
        internal void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public string WindowBackgroundColor
        {
            get
            {
                return SettingsService.CurrentTheme == Core.Enums.ThemeEnum.Light ? "#fff" : "#383737";
            }
        }

        public string BorderBackgroundColor
        {
            get
            {
                return SettingsService.CurrentTheme == Core.Enums.ThemeEnum.Light ? "#e1dede" : "#2e2e2e";
            }
        }

        public string FontColor
        {
            get
            {
                return SettingsService.CurrentTheme == Core.Enums.ThemeEnum.Light ? "#000" : "#fff";
            }
        }

        //Extra Stuff, shows why a base ViewModel is useful
        bool? _CloseWindowFlag;
        public bool? CloseWindowFlag
        {
            get { return _CloseWindowFlag; }
            set
            {
                _CloseWindowFlag = value;
                RaisePropertyChanged("CloseWindowFlag");
            }
        }

        public virtual void CloseWindow(bool? result = true)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                CloseWindowFlag = CloseWindowFlag == null
                    ? true
                    : !CloseWindowFlag;
            }));
        }

        public void NotifyWindowToClose()
        {
            Messenger.Default.Send(new NotificationMessage(this, "CloseWindowsBoundToMe"));
        }

        public void NotifyWindowToHide()
        {
            Messenger.Default.Send(new NotificationMessage(this, "HideWindow"));
        }
    }
}
