using Avalonia.Media.Imaging;
using CustomScreenBackgrounds.Util;
using HarmonyLib;
using Keen.Game2;
using Keen.Game2.Client.RuntimeSystems;
using Keen.Game2.Client.UI.Library;
using Keen.Game2.Client.UI.Menu.Background;
using Keen.VRage.Core;
using Keen.VRage.Core.Render;
using Keen.VRage.DCS.Components;
using Keen.VRage.Library.Filesystem;
using Keen.VRage.Library.Utils;
using Keen.VRage.UI.EngineComponents;
using Keen.VRage.UI.Screens;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomScreenBackgrounds.Patches
{
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
            (Singleton<VRageCore>.Instance.Engine.Get<UIEngineComponent>().ScreenManagerInternal._loadedScreens.FirstOrDefault(x => x.GetType() == typeof(MainMenuBackgroundScreen)).DataContext as MainMenuBackgroundScreenViewModel).BackgroundBitmap = PluginFileSystem.GetRandomImageFromDir(PluginFileSystem.MainMenuImagesFolderPath).Asset;
        }
    }
}
