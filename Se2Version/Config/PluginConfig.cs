using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CustomScreenBackgrounds.Config
{
    public class PluginConfig : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void SetValue<T>(ref T field, T value, [CallerMemberName] string propName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return;

            field = value;

            OnPropertyChanged(propName);
        }

        private void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;

            propertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private bool mainMenuOverlay = false;
        private bool mainMenuOverlay2 = false;
        private bool loadingScreenOverlay = false;
        private bool cleanLoadingMenu = true;
        private bool customMainMenuOverlay = false;
        private bool customLoadingMenuOverlay = false;
        private bool showloadingScreenPercent = true;

        public bool MainMenuOverlay 
        { 
            get => mainMenuOverlay; 
            set => SetValue(ref mainMenuOverlay, value); 
        }

        public bool MainMenuOverlay2 
        { 
            get => mainMenuOverlay2; 
            set => SetValue(ref mainMenuOverlay2, value); 
        }

        public bool LoadingScreenOverlay 
        { 
            get => loadingScreenOverlay; 
            set => SetValue(ref loadingScreenOverlay, value); 
        }

        public bool CleanLoadingMenu 
        { 
            get => cleanLoadingMenu; 
            set => SetValue(ref cleanLoadingMenu, value); 
        }

        public bool CustomMainMenuOverlay
        {
            get => customMainMenuOverlay;
            set => SetValue(ref customMainMenuOverlay, value);
        }

        public bool CustomLoadingMenuOverlay
        {
            get => customLoadingMenuOverlay;
            set => SetValue(ref customLoadingMenuOverlay, value);
        }
        public bool ShowLoadingMenuPercent 
        { 
            get => showloadingScreenPercent; 
            set => SetValue(ref showloadingScreenPercent, value); 
        }
    }
}