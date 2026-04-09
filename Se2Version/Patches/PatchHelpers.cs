using HarmonyLib;
using CustomScreenBackgrounds.Logging;
using System;

namespace CustomScreenBackgrounds.Patches;

// ReSharper disable once UnusedType.Global
public static class PatchHelpers
{
    public static bool HarmonyPatchEarly(PluginLogger log, Harmony harmony)
    {
#if DEBUG
        Harmony.DEBUG = true;
#endif

        log.Debug("Applying early Harmony patches");
        try
        {
            harmony.PatchCategory("Early");
        }
        catch (Exception ex)
        {
            log.Critical(ex, "Failed to apply early Harmony patches");
            return false;
        }

        return true;
    }

    public static bool HarmonyPatchLate(PluginLogger log, Harmony harmony)
    {
#if DEBUG
        Harmony.DEBUG = true;
#endif

        log.Debug("Applying late Harmony patches");
        try
        {
            harmony.PatchCategory("Late");
        }
        catch (Exception ex)
        {
            log.Critical(ex, "Failed to apply late Harmony patches");
            return false;
        }

        return true;
    }
}