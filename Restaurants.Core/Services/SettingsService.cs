using Microsoft.Win32;
using Restaurants.Core.Enums;
using System;

namespace Restaurants.Core.Services
{
    public class SettingsService
    {
        const string userRoot = "HKEY_CURRENT_USER";
        const string subkey = "Restaurants";
        const string keyName = userRoot + "\\" + subkey;

        public static ThemeEnum CurrentTheme
        {
            get
            {
                return GetCurrentTheme();
            }
            set
            {
                SetCurrentTheme(value);
            }
        }

        // получение темы с реестра
        static ThemeEnum GetCurrentTheme()
        {
            var currentTheme = Registry.GetValue(keyName, "Theme", null);

            if (currentTheme != null)
                return (currentTheme as string) == "Light" ? ThemeEnum.Light : ThemeEnum.Dark;

            Registry.SetValue(keyName, "Theme", "Light");
            return ThemeEnum.Light;
        }

        // сохранение темы в реестр
        static void SetCurrentTheme(ThemeEnum theme)
        {
            if(theme == ThemeEnum.Light)
                Registry.SetValue(keyName, "Theme", "Light");
            else
                Registry.SetValue(keyName, "Theme", "Dark");
        }
    }
}
