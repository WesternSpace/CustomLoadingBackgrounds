using CustomScreenBackgrounds.Logging;
using Keen.VRage.Library.Filesystem;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CustomScreenBackgrounds.Config;

// Ported from Torch's Persistent<T> class for compatibility of configuration files between targets and to work with IPluginLogger.
// Simple class that manages saving <see cref="P:Torch.Persistent`1.Data" /> to disk using XML serialization.
// Can automatically save on changes by implementing <see cref="T:System.ComponentModel.INotifyPropertyChanged" /> in the data class.
/// <typeparam name="T">Data class type</typeparam>
public class PersistentConfig<T> : IDisposable where T : class, INotifyPropertyChanged, new()
{
    private T data;
    private Timer saveConfigTimer;
    private const int SaveDelay = 500;

    private string Path { get; }
    private LocalFileSystem fileSystem;

    public T Data
    {
        get => data;
        private set
        {
            if (data != null)
                data.PropertyChanged -= OnPropertyChanged;

            data = value;
            data.PropertyChanged += OnPropertyChanged;
        }
    }

    ~PersistentConfig() => Dispose();

    private PersistentConfig(string path, T data = null, LocalFileSystem fileSystem = null)
    {
        Path = path;
        Data = data;
        this.fileSystem = fileSystem;
    }

    private void SaveLater()
    {
        if (saveConfigTimer == null)
            saveConfigTimer = new Timer(x => Save());

        saveConfigTimer.Change(SaveDelay, -1);
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e) => SaveLater();

    public static PersistentConfig<T> Load(PluginLogger log, LocalFileSystem system, string path)
    {
        try
        {
            if (system.FileExists(path))
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                using (var streamReader = new StreamReader(system.OpenRead(path, FileShare.Read)))
                    return new PersistentConfig<T>(path, (T)xmlSerializer.Deserialize(streamReader), system);
            }
        }
        catch (Exception e)
        {
            log.Error(e, "Failed to load configuration file: {0}", path);
            try
            {
                var timestamp = DateTime.Now.ToString("yyyyMMdd-hhmmss");
                var corruptedPath = $"{path}.corrupted.{timestamp}.txt";
                log.Info("Moving corrupted configuration file: {0} => {1}", path, corruptedPath);
                system.MoveFile(path, corruptedPath);
            }
            catch (Exception)
            {
                // Ignored
            }
        }

        log.Info("Writing default configuration file: {0}", path);
        var config = new PersistentConfig<T>(path, new T(), system);
        config.Save();
        return config;
    }

    private void Save(string path = null)
    {
        if (path == null)
            path = Path;

        // NOTE: There is a minimal chance of inconsistency here if the config data
        // is changed concurrently, but it is negligible in practice. Also, it would be
        // corrected by the next scheduled save operation after SaveDelay milliseconds.
        using (var text = new StreamWriter(fileSystem.Open(path, FileMode.Create, FileAccess.Write)))
            new XmlSerializer(typeof(T)).Serialize(text, Data);
    }

    public void Dispose()
    {
        try
        {
            if (Data is INotifyPropertyChanged d)
                d.PropertyChanged -= OnPropertyChanged;

            saveConfigTimer?.Dispose();
            Save();
        }
        catch
        {
            // Ignored
        }
    }
}