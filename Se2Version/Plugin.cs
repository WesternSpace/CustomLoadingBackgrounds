using CustomScreenBackgrounds.Config;
using CustomScreenBackgrounds.Logging;
using CustomScreenBackgrounds.Patches;
using CustomScreenBackgrounds.Util;
using HarmonyLib;
using Keen.Game2.Game.Plugins;
using System;
using System.IO;

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
        Log.Info("SE2-test build");

        if (!PatchHelpers.HarmonyPatchEarly(Log, new Harmony(Name +".Early")))
        {
            Log.Debug("Failed to load");
            return;
        }

        Log.Debug("Successfully loaded");
    }

    public void Init()
    {
        if (InitDone)
            return;

        PluginFileSystem.Init();

        config = PersistentConfig<PluginConfig>.Load(Log, PluginFileSystem.RootFolder, Path.Combine(PluginFileSystem.ConfigFolderPath, ConfigFileName));


        if (!PatchHelpers.HarmonyPatchLate(Log, new Harmony(Name)))
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
