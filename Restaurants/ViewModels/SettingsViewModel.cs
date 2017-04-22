using Restaurants.Core.Enums;
using Restaurants.Core.Services;
using System;
using System.Collections.ObjectModel;

namespace Restaurants.ViewModels
{
    class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            Themes = new ObservableCollection<string>(Enum.GetNames(typeof(ThemeEnum)));
        }

        public ObservableCollection<string> Themes { get; set; }

        public string CurrentTheme
        {
            get
            {
                return SettingsService.CurrentTheme == ThemeEnum.Light ? "Light" : "Dark";
            }
            set
            {
                if (value == "Light")
                    SettingsService.CurrentTheme = ThemeEnum.Light;
                else if (value == "Dark")
                    SettingsService.CurrentTheme = ThemeEnum.Dark;

                RaisePropertyChanged("WindowBackgroundColor");
                RaisePropertyChanged("BorderBackgroundColor");
                RaisePropertyChanged("FontColor");
                RaisePropertyChanged("CurrentTheme");
            }
        }
    }
}
