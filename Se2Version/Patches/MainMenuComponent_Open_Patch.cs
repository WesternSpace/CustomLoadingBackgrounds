using Avalonia.Controls;
using CustomScreenBackgrounds.Util;
using HarmonyLib;
using Keen.Game2.Client.RuntimeSystems;
using Keen.Game2.Client.UI.Menu;
using Keen.Game2.Client.UI.Menu.Background;
using Keen.Game2.Client.UI.Menu.MainMenu;
using Keen.Game2.Client.UI.Menu.News;
using Keen.VRage.UI.Shared.Extensions;

namespace CustomScreenBackgrounds.Patches;

[HarmonyPatchCategory("Early")]
[HarmonyPatch(typeof(MainMenuComponent), "Open")]
internal class MainMenuComponent_Open_Patch
{
    private static void Prefix()
    {
        Plugin.Instance?.Init();
    }

    private static void Postfix()
    {
        MainMenuBackgroundScreen? bgScreen = UiTools.GetScreenManager()?.GetScreenofType<MainMenuBackgroundScreen>();

        if (PluginFileSystem.GetAllMainMenuScreenImageFiles().Count() != 0)
            (bgScreen?.DataContext as MainMenuBackgroundScreenViewModel)?.
                BackgroundBitmap = PluginFileSystem.GetRandomImageFromDir(PluginFileSystem.MainMenuImagesFolderPath)?.Asset;

        MainMenuScreen? mainMenu = UiTools.GetScreenManager()?.GetScreenofType<MainMenuScreen>();

        mainMenu?.FindChildOfType<Image>("PART_HighlightPresenter")?.
            IsVisible = !Plugin.Instance.Config.HideRightImage;

        mainMenu?.FindChildOfType<News>("PART_NewsPresenter").
            IsVisible = !Plugin.Instance.Config.HideRightImage;

        GameMenu? gameMenu = mainMenu?.FindChildOfType<GameMenu>();

        gameMenu?.FindChildOfType<Grid>()?.
            FindChildOfTypeNonRecursive<Image>()?
            .IsVisible = !Plugin.Instance.Config.HideMainMenuKeenLogo;

        gameMenu?.FindChildOfType<Border>("PART_KeenLogoHit")?.
            IsVisible = !Plugin.Instance.Config.HideMainMenuKeenLogo;

        gameMenu?.FindChildOfType<Grid>()?.
            FindChildOfTypeNonRecursive<Grid>()?.
            IsVisible = !Plugin.Instance.Config.HideMainMenuButtonsBackground;

        if (Plugin.Instance.Config.HideMainMenuButtonsBackground)
        {
            gameMenu?.FindChildOfType<Grid>()?.
                FindChildOfTypeNonRecursive<Canvas>()?.
                FindChildOfTypeNonRecursive<Grid>()?[Canvas.LeftProperty] = 40.0;
        }

        gameMenu?.FindChildOfType<Grid>()?.
            FindChildOfTypeNonRecursive<TextBlock>()?.
            IsVisible = !Plugin.Instance.Config.HideMainMenuVersionString;

        //TODO: Inject image for overlay

        if (Plugin.Instance.Config.SmallerMainMenu)
        {
            gameMenu?.FindChildOfType<Grid>("PART_PreviewPresenter")?.Width = 289.0;
            gameMenu?.FindChildOfType<Grid>("PART_PreviewPresenter")?.Height = 192.0;

            gameMenu?.FindChildOfType<Grid>("PART_PreviewPresenter")?.
                FindChildOfTypeNonRecursive<Grid>()?.Width = 289.0;

            gameMenu?.FindChildOfType<Grid>("PART_PreviewPresenter")?.
                FindChildOfTypeNonRecursive<Grid>()?.Height = 160.0;

            (gameMenu?.FindChildOfType<Grid>("PART_PreviewPresenter")?.
                FindChildOfType<TextBlock>()?.Parent as Grid)?.Height = 30.0;

            gameMenu?.FindChildOfType<Grid>("PART_PreviewPresenter")?.
                FindChildOfType<TextBlock>()?.FontSize = 16.0;
        }
    }
}
