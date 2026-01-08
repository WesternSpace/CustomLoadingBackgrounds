using Keen.Game2.Client.UI.Library;
using Keen.VRage.Library.Filesystem;
using Keen.VRage.Library.Utils;
using System.Security.Cryptography;

namespace CustomScreenBackgrounds.Util;
internal static class PluginFileSystem
{
    public static LocalFileSystem RootFolder;

    public const string MainMenuImagesFolderPath = "MainMenuScreenBackgroundImages";
    public const string MainMenuVideosFolderPath = "MainMenuScreenBackgroundVideos";
    public const string LoadingMenuImagesFolderPath = "LoadingMenuImages";
    public const string MainMenuCustomOverlaysFolderPath = "CustomOverlays/MainMenu";
    public const string LoadingMenuCustomOverlaysFolderPath = "CustomOverlays/LoadingMenu";
    public const string ConfigFolderPath = "Config";

    private static readonly List<string> allowedImageFileExtensions = new List<string> { "jpg", "png" };
    public static void Init()
    {
        if (!Singleton<FileSystem>.Instance.AppDataFiles.DirectoryExists("CustomScreenBackgrounds"))
        {
            Singleton<FileSystem>.Instance.AppDataFiles.CreateDirectory("CustomScreenBackgrounds");
        }

        RootFolder = new LocalFileSystem("CustomScreenBackgrounds", Singleton<FileSystem>.Instance.AppDataFiles, false);

        if (!RootFolder.DirectoryExists(MainMenuImagesFolderPath))
        {
            RootFolder.CreateDirectory(MainMenuImagesFolderPath);
        }

        if (!RootFolder.DirectoryExists(MainMenuVideosFolderPath))
        {
            RootFolder.CreateDirectory(MainMenuVideosFolderPath);
        }

        if (!RootFolder.DirectoryExists(LoadingMenuImagesFolderPath))
        {
            RootFolder.CreateDirectory(LoadingMenuImagesFolderPath);
        }

        if (!RootFolder.DirectoryExists(MainMenuCustomOverlaysFolderPath))
        {
            RootFolder.CreateDirectory(MainMenuCustomOverlaysFolderPath);
        }

        if (!RootFolder.DirectoryExists(LoadingMenuCustomOverlaysFolderPath))
        {
            RootFolder.CreateDirectory(LoadingMenuCustomOverlaysFolderPath);
        }
        if (!RootFolder.DirectoryExists(ConfigFolderPath))
        {
            RootFolder.CreateDirectory(ConfigFolderPath);
        }
    }

    public static RuntimeGUIAsset? GetRandomImageFromDir(string path)
    {
        RuntimeGUIAsset? asset = null;
        if (!string.IsNullOrEmpty(path))
        {
            try
            {
                IEnumerable<string> fileNames = RootFolder.EnumerateFiles(path).Where(s => allowedImageFileExtensions.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant()));
                RandomNumberGenerator rng = RandomNumberGenerator.Create();
                byte[] data = new byte[4];
                rng.GetBytes(data);
                int value = BitConverter.ToInt32(data, 0);
                Random R = new Random(value);


                string fileName = fileNames.ElementAt(R.Next(0, fileNames.Count()));

                FileHandle handle = FileHandle.CreateNormalizedFileHandle(RootPath.AppData, FileSystemHelpers.Combine(PluginFileSystem.RootFolder.BasePath,fileName));

                asset = RuntimeGUIAssetService.Instance.GetAsset(handle);
            }
            catch
            {
            }
        }

        
        return asset;
    }

    public static IEnumerable<string> GetAllLoadingScreenFiles()
    {
        return RootFolder.EnumerateFiles(LoadingMenuImagesFolderPath).Where(s => allowedImageFileExtensions.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant()));
    }

    public static IEnumerable<string> GetAllMainMenuScreenImageFiles()
    {
        return RootFolder.EnumerateFiles(MainMenuImagesFolderPath).Where(s => allowedImageFileExtensions.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant()));
    }

    public static IEnumerable<string> GetAllMainMenuScreenVideoFiles()
    {
        return RootFolder.EnumerateFiles(MainMenuVideosFolderPath).Where(s => Path.GetExtension(s).TrimStart('.').Equals("wmv", StringComparison.InvariantCultureIgnoreCase));
    }
}
