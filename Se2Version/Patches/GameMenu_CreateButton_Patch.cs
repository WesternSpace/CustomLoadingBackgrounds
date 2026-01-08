using Avalonia.Controls;
using HarmonyLib;
using Keen.Game2.Client.UI.Menu;
using System.Windows.Input;

namespace CustomScreenBackgrounds.Patches
{
    [HarmonyPatchCategory("Early")]
    [HarmonyPatch(typeof(GameMenu), "CreateButton", [typeof(string), typeof(ICommand)])]
    internal class GameMenu_CreateButton_Patch
    {
        private static void Postfix(ref Button __result)
        {
            if (!Plugin.Instance.Config.SmallerMainMenu)
                return;

            __result.Height = 40.0;
            __result.Width = 250.0;
        }
    }
}
