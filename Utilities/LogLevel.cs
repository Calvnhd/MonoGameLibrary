namespace MonoGameLibrary.Utilities;

/// <summary>
/// Defines the severity levels for log messages.
/// Ordered by severity; integer values enable comparison filtering via <see cref="GameLogger.MinimumLevel"/>.
/// </summary>
public enum LogLevel
{
    /// <summary>Verbose per-frame diagnostics. Compiled out in Release via [Conditional("DEBUG")].</summary>
    Trace = 0,

    /// <summary>Lifecycle events and state changes (scene loads, initialization).</summary>
    Info = 1,

    /// <summary>Unexpected but recoverable situations (missing texture, fallback used).</summary>
    Warning = 2,

    /// <summary>Broken or failed operations (load failure, null reference caught).</summary>
    Error = 3,

    /// <summary>Not a loggable level. Used only as a <see cref="GameLogger.MinimumLevel"/> value to suppress all output.</summary>
    None = 4
}
