using Avalonia.Controls;
using CustomScreenBackgrounds.Util;
using Keen.Game2.Client.UI.Menu;
using Keen.Game2.Client.UI.Menu.MainMenu;
using Keen.Game2.Client.UI.Menu.News;
using Keen.VRage.UI.Shared.Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CustomScreenBackgrounds.Config;

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

    private bool hideRightImage = true;
    private bool hideNews = true;
    private bool hideMainMenuKeenLogo = true;
    private bool hideMainMenuButtonsBackground = true;
    private bool hideMainMenuVersionString = false;
    private bool customMainMenuOverlay = false;
    private bool smallerMainMenu = true;

    private bool cleanLoadingMenu = true;
    
    private bool customLoadingMenuOverlay = false;
    private bool showloadingScreenPercent = true;

    public bool HideRightImage 
    { 
        get => hideRightImage;
        set
        {
            UiTools.GetScreenManager()?.
                GetScreenofType<MainMenuScreen>()?.
                FindChildOfType<Image>("PART_HighlightPresenter")?.
                IsVisible = !value;
            
            SetValue(ref hideRightImage, value);
        }
    }

    public bool HideNews 
    { 
        get => hideNews;
        set
        {
            UiTools.GetScreenManager()?.
               GetScreenofType<MainMenuScreen>()?.
               FindChildOfType<News>("PART_NewsPresenter")?.
               IsVisible = !value;

            SetValue(ref hideNews, value);
        }
    }

    public bool HideMainMenuKeenLogo 
    { 
        get => hideMainMenuKeenLogo;
        set
        {
            UiTools.GetScreenManager()?.
               GetScreenofType<MainMenuScreen>()?.
               FindChildOfType<GameMenu>()?.
               FindChildOfType<Grid>()?.
               FindChildOfTypeNonRecursive<Image>()?.IsVisible = !value;

            UiTools.GetScreenManager()?.
               GetScreenofType<MainMenuScreen>()?.
               FindChildOfType<Border>("PART_KeenLogoHit")?.IsVisible = !value;

            SetValue(ref hideMainMenuKeenLogo, value); 
        }
    }

    public bool HideMainMenuButtonsBackground
    {
        get => hideMainMenuButtonsBackground;
        set
        {
            UiTools.GetScreenManager()?.
               GetScreenofType<MainMenuScreen>()?.
               FindChildOfType<GameMenu>()?.
               FindChildOfType<Grid>()?.
               FindChildOfTypeNonRecursive<Grid>()?.IsVisible = !value;

            if (value)
            {
                UiTools.GetScreenManager()?.
                    GetScreenofType<MainMenuScreen>()?.
                    FindChildOfType<GameMenu>()?.
                    FindChildOfType<Grid>()?.
                    FindChildOfTypeNonRecursive<Canvas>()?.
                    FindChildOfTypeNonRecursive<Grid>()?[Canvas.LeftProperty] = 40f;
            }
            else
            {
                UiTools.GetScreenManager()?.
                    GetScreenofType<MainMenuScreen>()?.
                    FindChildOfType<GameMenu>()?.
                    FindChildOfType<Grid>()?.
                    FindChildOfTypeNonRecursive<Canvas>()?.
                    FindChildOfTypeNonRecursive<Grid>()?[Canvas.LeftProperty] = 116f;
            }


            SetValue(ref hideMainMenuButtonsBackground, value);
        }
    }

    public bool HideMainMenuVersionString
    {
        get => hideMainMenuVersionString;
        set
        {
            UiTools.GetScreenManager()?.
                GetScreenofType<MainMenuScreen>()?.
                FindChildOfType<GameMenu>()?.
                FindChildOfType<Grid>()?.
                FindChildOfTypeNonRecursive<TextBlock>()?.IsVisible = !value;

            SetValue(ref hideMainMenuVersionString, value);
        }
    }

    public bool CustomMainMenuOverlay
    {
        get => customMainMenuOverlay;
        set => SetValue(ref customMainMenuOverlay, value);
    }

    public bool SmallerMainMenu
    {
        get => smallerMainMenu;
        set => SetValue(ref smallerMainMenu, value);
    }

    public bool CleanLoadingMenu 
    { 
        get => cleanLoadingMenu; 
        set => SetValue(ref cleanLoadingMenu, value); 
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