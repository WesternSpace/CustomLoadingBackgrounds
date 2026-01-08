using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Keen.VRage.Core;
using Keen.VRage.Library.Utils;
using Keen.VRage.UI.EngineComponents;
using Keen.VRage.UI.Screens;

namespace CustomScreenBackgrounds.Util;
internal static class UiTools
{
    public static ScreenManager? GetScreenManager()
    {
        return Singleton<VRageCore>.Instance.Engine.Get<UIEngineComponent>().ScreenManagerInternal;
    }

    public static T? GetScreenofType<T>(this ScreenManager sm) where T : ScreenView
    {
        return (T?)sm._loadedScreens.FirstOrDefault(x => x.GetType() == typeof(T));
    }

    public static T? FindChildOfTypeNonRecursive<T>(this Control control, string name = "") where T : Control
    {
        IAvaloniaList<Visual> avaloniaList = (IAvaloniaList<Visual>)control.GetVisualChildren();
        foreach (Visual item in avaloniaList)
        {
            if (item is Control)
            {
                if (item is T v && (string.IsNullOrEmpty(name) || v.Name == name))
                {
                    return v;
                }
            }
        }

        return null;
    }
}
