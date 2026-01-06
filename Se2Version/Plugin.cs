using CustomScreenBackgrounds.Config;
using CustomScreenBackgrounds.Logging;
using CustomScreenBackgrounds.Patches;
using CustomScreenBackgrounds.Util;
using HarmonyLib;
using Keen.Game2.Game.Plugins;
using Keen.VRage.Core.EngineComponents;
using Keen.VRage.Core.Project;
using Keen.VRage.Library.Filesystem;
using Keen.VRage.Library.Utils;
using System.Reflection;

namespace CustomScreenBackgrounds;

public class Plugin : IPlugin, IDisposable
{
    public const string Name = "CustomMenuBackgrounds";
    public static Plugin? Instance { get; private set; }

    public PluginLogger Log => Logger;
    private static readonly PluginLogger Logger = new PluginLogger(Name);

    public PluginConfig? Config => config?.Data;
    private PersistentConfig<PluginConfig>? config;
    private static readonly string ConfigFileName = $"{Name}.cfg";

    public bool InitDone = false;

    public Plugin(PluginHost host)
    {
        Instance = this;

        Log.Info("Loading");

        new Harmony(Name + ".Early").PatchCategory("Early");

        Log.Debug("Successfully loaded");
    }

    public void Init()
    {
        if (InitDone)
            return;

        PluginFileSystem.Init();

        config = PersistentConfig<PluginConfig>.Load(Log, PluginFileSystem.RootFolder, Path.Combine(PluginFileSystem.ConfigFolderPath, ConfigFileName));


        if (!PatchHelpers.HarmonyPatchAll(Log, new Harmony(Name)))
        {
            return;
        }

        InitDone = true;
    }

    public void Dispose()
    {
        Instance = null;
    }
}
