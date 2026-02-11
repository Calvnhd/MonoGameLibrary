using System;
using System.Diagnostics;
using System.IO;

#nullable enable

namespace MonoGameLibrary.Utilities;

/// <summary>
/// Lightweight logger with dual output (console + file).
/// Provides four severity methods: Trace, Info, Warn, Error.
/// Trace calls are compiled out of Release builds via [Conditional("DEBUG")].
/// </summary>
public static class GameLogger
{
    // ─── Static Fields ───

    // Pre-built padded labels indexed by (int)LogLevel. Avoids Enum.ToString() allocation.
    private static readonly string[] s_levelLabels = { "TRACE", "INFO ", "WARN ", "ERROR" };

    // Buffered file writer for logs/game.log. Created lazily on first log call.
    private static StreamWriter? s_writer;

    // Guards EnsureInitialized() so it runs exactly once.
    private static bool s_initialized;

    // Synchronization lock for initialization and shutdown.
    private static readonly object s_lock = new object();

    // ─── Public Properties ───

    /// <summary>
    /// Messages with level below this value are discarded before formatting.
    /// Set to <see cref="LogLevel.None"/> to suppress all output.
    /// </summary>
    public static LogLevel MinimumLevel { get; set; } = LogLevel.Trace;

    // ─── Public Methods ───

    /// <summary>Log verbose per-frame diagnostic output. Compiled out in Release builds.</summary>
    [Conditional("DEBUG")]
    public static void Trace(string category, string message)
    {
        // TODO: Delegate to Log(LogLevel.Trace, category, message)
    }

    /// <summary>Log noteworthy lifecycle events and state changes.</summary>
    public static void Info(string category, string message)
    {
        // TODO: Delegate to Log(LogLevel.Info, category, message)
    }

    /// <summary>Log unexpected but recoverable situations.</summary>
    public static void Warn(string category, string message)
    {
        // TODO: Delegate to Log(LogLevel.Warning, category, message)
    }

    /// <summary>Log broken or failed operations.</summary>
    public static void Error(string category, string message)
    {
        // TODO: Delegate to Log(LogLevel.Error, category, message)
    }

    /// <summary>Flush and close the log file. Called by Core.UnloadContent() during game exit.</summary>
    public static void Shutdown()
    {
        // TODO: Lock on s_lock
        //   If s_writer is not null: Flush, Dispose, set to null
        //   Keep s_initialized = true so EnsureInitialized won't re-open the file
    }

    // ─── Private Methods ───

    private static void Log(LogLevel level, string category, string message)
    {
        // TODO: If level < MinimumLevel, return immediately
        // TODO: Call EnsureInitialized()
        // TODO: Build formatted string via FormatMessage(level, category, message)
        // TODO: Write to Console.WriteLine
        // TODO: If s_writer is not null, write to s_writer.WriteLine
    }

    private static void EnsureInitialized()
    {
        // TODO: If s_initialized, return immediately
        // TODO: Lock on s_lock, double-check s_initialized
        // TODO: Directory.CreateDirectory("logs")
        // TODO: Open StreamWriter on "logs/game.log", append=true, AutoFlush=false
        // TODO: Write session separator header
        // TODO: Hook AppDomain.CurrentDomain.ProcessExit += OnProcessExit
        // TODO: Set s_initialized = true
    }

    private static string FormatMessage(LogLevel level, string category, string message)
    {
        // TODO: Get timestamp: DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
        // TODO: Get level label: s_levelLabels[(int)level]
        // TODO: Pad category to 10 chars: category.PadRight(10)
        // TODO: Return: "{timestamp} [{label}] {paddedCategory} | {message}"
        return string.Empty;
    }

    private static void OnProcessExit(object? sender, EventArgs e)
    {
        // TODO: Call Shutdown() as fallback flush
    }
}
