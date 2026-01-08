using Keen.VRage.Library.Diagnostics;
using System.Runtime.CompilerServices;

namespace CustomScreenBackgrounds.Logging;

public class PluginLogger : LogFormatter
{
    public PluginLogger(string pluginName) : base($"{pluginName}: ")
    {
    }

    public bool IsTraceEnabled => Log.Default != null;
    public bool IsDebugEnabled => Log.Default != null;
    public bool IsInfoEnabled => Log.Default != null;
    public bool IsWarningEnabled => Log.Default != null;
    public bool IsErrorEnabled => Log.Default != null;
    public bool IsCriticalEnabled => Log.Default != null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Trace(Exception ex, string message, params object[] data)
    {
        if (!IsTraceEnabled)
            return;
        // Keen does not have a Trace log level, using Debug instead
        Log.Default.Debug(Format(ex, message, data));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Debug(Exception ex, string message, params object[] data)
    {
        if (!IsDebugEnabled)
            return;

        Log.Default.Debug(Format(ex, message, data));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Info(Exception ex, string message, params object[] data)
    {
        if (!IsInfoEnabled)
            return;

        Log.Default.Info(Format(ex, message, data));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Warning(Exception ex, string message, params object[] data)
    {
        if (!IsWarningEnabled)
            return;

        Log.Default.Warning(Format(ex, message, data));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Error(Exception ex, string message, params object[] data)
    {
        if (!IsErrorEnabled)
            return;

        Log.Default.Error(Format(ex, message, data));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Critical(Exception ex, string message, params object[] data)
    {
        if (!IsCriticalEnabled)
            return;

        Log.Default.Critical(Format(ex, message, data));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Trace(string message, params object[] data)
    {
        Trace(null, message, data);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Debug(string message, params object[] data)
    {
        Debug(null, message, data);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Info(string message, params object[] data)
    {
        Info(null, message, data);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Warning(string message, params object[] data)
    {
        Warning(null, message, data);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Error(string message, params object[] data)
    {
        Error(null, message, data);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Critical(string message, params object[] data)
    {
        Critical(null, message, data);
    }
}