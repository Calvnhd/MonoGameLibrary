using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;

#nullable enable

namespace MonoGameLibrary.Utilities;

/// <summary>
/// Synchronous terminal and buffered current-run file logging.
/// Expected sink failures disable that sink without preventing the other output.
/// File output uses BOM-free UTF-8 and replaces malformed diagnostic text.
/// </summary>
public static class GameLogger
{
    private static readonly string[] s_levelLabels = ["TRACE", "INFO ", "WARN ", "ERROR"];
    private static readonly object s_lock = new();
    private static StreamWriter? s_writer;
    private static bool s_initialized;
    private static bool s_shutdown;
    private static bool s_terminalAvailable = true;
#if DEBUG
    private static LogLevel s_minimumLevel = LogLevel.Trace;
#else
    private static LogLevel s_minimumLevel = LogLevel.Info;
#endif

    /// <summary>Gets or sets the minimum severity. None suppresses normal entries, not sink failures.</summary>
    public static LogLevel MinimumLevel
    {
        get
        {
            lock (s_lock)
                return s_minimumLevel;
        }
        set
        {
            if (value < LogLevel.Trace || value > LogLevel.None)
                throw new ArgumentOutOfRangeException(nameof(value));

            lock (s_lock)
                s_minimumLevel = value;
        }
    }

    /// <summary>Opens a host-selected current-run file once, allowing readers but excluding other writers.</summary>
    /// <param name="absoluteFilePath">Fully qualified log file path; no working-directory fallback is used.</param>
    /// <returns>True if the file opened; false if an operational failure left logging terminal-only.</returns>
    /// <exception cref="ArgumentException">The path is empty or not fully qualified.</exception>
    /// <exception cref="InvalidOperationException">Initialization or shutdown already occurred.</exception>
    public static bool Initialize(string absoluteFilePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(absoluteFilePath);
        if (!Path.IsPathFullyQualified(absoluteFilePath))
            throw new ArgumentException("The log file path must be fully qualified.", nameof(absoluteFilePath));
        var directory = Path.GetDirectoryName(absoluteFilePath);
        if (string.IsNullOrEmpty(directory))
            throw new ArgumentException("The log file path must name a file, not a filesystem root.", nameof(absoluteFilePath));

        lock (s_lock)
        {
            if (s_initialized || s_shutdown)
                throw new InvalidOperationException("The logger cannot be initialized again.");

            s_initialized = true;
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            try
            {
                Directory.CreateDirectory(directory);
                s_writer = new StreamWriter(new FileStream(
                    absoluteFilePath, FileMode.Create, FileAccess.Write, FileShare.Read),
                    new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false));
                return true;
            }
            catch (Exception error) when (IsFileFailure(error))
            {
                ReportFailures(null, error);
                return false;
            }
        }
    }

    /// <summary>Logs temporary verbose diagnostics. The call and its arguments are omitted without DEBUG.</summary>
    /// <param name="category">Free-form diagnostic category.</param>
    /// <param name="message">Diagnostic text.</param>
    [Conditional("DEBUG")]
    public static void Trace(string category, string message) => Log(LogLevel.Trace, category, message);

    /// <summary>Logs a lifecycle event or state change.</summary>
    /// <param name="category">Free-form diagnostic category.</param>
    /// <param name="message">Diagnostic text.</param>
    public static void Info(string category, string message) => Log(LogLevel.Info, category, message);

    /// <summary>Logs an unexpected but recoverable situation.</summary>
    /// <param name="category">Free-form diagnostic category.</param>
    /// <param name="message">Diagnostic text.</param>
    public static void Warn(string category, string message) => Log(LogLevel.Warning, category, message);

    /// <summary>Logs and immediately flushes a failed operation.</summary>
    /// <param name="category">Free-form diagnostic category.</param>
    /// <param name="message">Diagnostic text.</param>
    public static void Error(string category, string message) => Log(LogLevel.Error, category, message);

    /// <summary>Logs and immediately flushes a failed operation with full exception details.</summary>
    /// <param name="category">Free-form diagnostic category.</param>
    /// <param name="message">Diagnostic text.</param>
    /// <param name="exception">Exception with stack and inner-exception details.</param>
    public static void Error(string category, string message, Exception? exception) =>
        Log(LogLevel.Error, category, exception is null ? message : $"{message}{Environment.NewLine}{exception}");

    /// <summary>Flushes and closes the file after resource disposal. Repeated calls are safe; reopening is prohibited.</summary>
    public static void Shutdown()
    {
        lock (s_lock)
        {
            if (s_shutdown)
                return;

            s_shutdown = true;
            var writer = s_writer;
            s_writer = null;
            if (writer is null)
                return;

            Exception? failure = null;
            try
            {
                writer.Flush();
            }
            catch (Exception error) when (IsFileFailure(error))
            {
                failure = error;
            }

            ReportFailures(null, DisposeWriter(writer, failure));
        }
    }

    private static void Log(LogLevel level, string category, string message)
    {
        lock (s_lock)
        {
            if (level < s_minimumLevel)
                return;

            var formatted = FormatMessage(level, category, message);
            var terminalFailure = WriteTerminal(formatted);
            var fileFailure = WriteFile(formatted, level == LogLevel.Error);
            ReportFailures(terminalFailure, fileFailure);
        }
    }

    private static Exception? WriteTerminal(string message, bool isSinkFailure = false)
    {
        if (!s_terminalAvailable)
            return null;

        try
        {
            var terminal = isSinkFailure ? Console.Error : Console.Out;
            terminal.WriteLine(message);
            terminal.Flush();
            return null;
        }
        catch (Exception error) when (error is IOException or ObjectDisposedException)
        {
            s_terminalAvailable = false;
            return error;
        }
    }

    private static Exception? WriteFile(string message, bool flush)
    {
        var writer = s_writer;
        if (writer is null)
            return null;

        try
        {
            writer.WriteLine(message);
            if (flush)
                writer.Flush();
            return null;
        }
        catch (Exception error) when (IsFileFailure(error))
        {
            s_writer = null;
            return DisposeWriter(writer, error);
        }
    }

    private static Exception? DisposeWriter(StreamWriter writer, Exception? failure)
    {
        try
        {
            writer.Dispose();
        }
        catch (Exception error) when (IsFileFailure(error))
        {
            return failure is null ? error : new AggregateException(failure, error);
        }

        return failure;
    }

    private static void ReportFailures(Exception? terminalFailure, Exception? fileFailure)
    {
        // Use only the surviving sink directly. Never recurse into normal logging or retry a failed sink.
        if (terminalFailure is not null)
        {
            var reportingFailure = WriteFile(FormatMessage(LogLevel.Error, "Logging",
                $"Terminal sink disabled for this run.{Environment.NewLine}{terminalFailure}"), true);
            fileFailure ??= reportingFailure;
        }

        if (fileFailure is not null)
            WriteTerminal(FormatMessage(LogLevel.Error, "Logging",
                $"File sink disabled for this run.{Environment.NewLine}{fileFailure}"), true);
    }

    private static bool IsFileFailure(Exception error) =>
        error is IOException or UnauthorizedAccessException or SecurityException;

    private static string FormatMessage(LogLevel level, string category, string message) =>
        $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)} "
        + $"[{s_levelLabels[(int)level]}] {category.PadRight(10)} | {message}";

    private static void OnProcessExit(object? sender, EventArgs e) => Shutdown();
}
