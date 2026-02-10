// Namespace: MonoGameLibrary.Utilities

// XML doc: Defines severity levels for log messages.
// Ordered by severity — integer values enable simple comparison filtering
// in GameLogger.MinimumLevel (level < MinimumLevel → discard).

// Enum LogLevel
    // Trace = 0       — Verbose per-frame diagnostics. Compiled out in Release via [Conditional("DEBUG")].
    // Info = 1        — Lifecycle events, state changes (scene loads, initialization).
    // Warning = 2     — Unexpected but recoverable situations (missing texture, fallback used).
    // Error = 3       — Broken or failed operations (load failure, null reference caught).
    // None = 4        — NOT a loggable level. Only used as a MinimumLevel value to suppress all output.
