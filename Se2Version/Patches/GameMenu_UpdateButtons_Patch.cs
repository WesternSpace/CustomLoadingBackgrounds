using Avalonia.Controls;
using HarmonyLib;
using Keen.Game2.Client.UI.Menu;
using Keen.VRage.UI.Shared.Extensions;

namespace CustomScreenBackgrounds.Patches;

[HarmonyPatchCategory("Early")]
[HarmonyPatch(typeof(GameMenu), "UpdateButtons")]
internal class GameMenu_UpdateButtons_Patch
{
    private static void Postfix(GameMenu __instance)
    {
        if (__instance._buttonsPanel == null)
            return;

        if (!Plugin.Instance.Config.SmallerMainMenu)
            return;

        IEnumerable<Control>? controls = __instance._buttonsPanel.
            FindChildrenOfType<Control>(RecursiveSearchMode.Disabled);

        foreach (Control control in controls)
        {
            if (control is Separator sep)
                sep.Height = 5f;
        }
    }
}
