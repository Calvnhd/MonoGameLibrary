// Using: System, System.Diagnostics, System.IO
// Namespace: MonoGameLibrary.Utilities

// XML doc: Lightweight logger with dual output (console + file).
// Provides four severity methods: Trace, Info, Warn, Error.
// Trace calls are compiled out of Release builds via [Conditional("DEBUG")].

// Static class GameLogger

    // ─── Static Fields ───

    // s_levelLabels: readonly string array { "TRACE", "INFO ", "WARN ", "ERROR" }
    //   — Pre-built padded labels indexed by (int)LogLevel. Avoids Enum.ToString() allocation.
    //   — None is excluded because it's never formatted into output.

    // s_writer: StreamWriter, nullable, initially null
    //   — Buffered file writer for logs/game.log. Created lazily on first log call.

    // s_initialized: bool, initially false
    //   — Guards EnsureInitialized() so it runs exactly once.

    // s_lock: readonly object = new object()
    //   — Synchronization lock for initialization and shutdown (AppDomain.ProcessExit runs on a different thread).

    // ─── Public Properties ───

    // MinimumLevel: LogLevel, get/set, default = LogLevel.Trace
    //   — Messages with level < MinimumLevel are discarded before formatting.
    //   — Set to LogLevel.None to suppress all output.

    // ─── Public Methods ───

    // [Conditional("DEBUG")]
    // Trace(string category, string message)
    //   — XML doc: Log verbose per-frame diagnostic output. Compiled out in Release builds.
    //   — Delegate to Log(LogLevel.Trace, category, message)

    // Info(string category, string message)
    //   — XML doc: Log noteworthy lifecycle events and state changes.
    //   — Delegate to Log(LogLevel.Info, category, message)

    // Warn(string category, string message)
    //   — XML doc: Log unexpected but recoverable situations.
    //   — Delegate to Log(LogLevel.Warning, category, message)

    // Error(string category, string message)
    //   — XML doc: Log broken or failed operations.
    //   — Delegate to Log(LogLevel.Error, category, message)

    // Shutdown()
    //   — XML doc: Flush and close the log file. Called by Core.UnloadContent() during game exit.
    //   — Lock on s_lock
    //     — If s_writer is not null:
    //       — Flush the writer
    //       — Dispose the writer
    //       — Set s_writer to null
    //     — Set s_initialized to false (prevent re-initialization after shutdown)
    //       — Actually, set to true so EnsureInitialized won't re-open the file.
    //       — After shutdown, Log() will skip the file write because s_writer is null.

    // ─── Private Methods ───

    // Log(LogLevel level, string category, string message)
    //   — If level < MinimumLevel, return immediately (no formatting, no allocation)
    //   — Call EnsureInitialized()
    //   — Build formatted string via FormatMessage(level, category, message)
    //   — Write formatted string to Console.WriteLine
    //   — If s_writer is not null, write formatted string to s_writer.WriteLine

    // EnsureInitialized()
    //   — If s_initialized is true, return immediately
    //   — Lock on s_lock
    //     — Double-check: if s_initialized is true, return (another thread may have initialized)
    //     — Set s_initialized to true
    //     — Create "logs" directory via Directory.CreateDirectory("logs")
    //     — Open StreamWriter on "logs/game.log" with append=true
    //     — Set AutoFlush = false (buffered writing to avoid per-line disk I/O)
    //     — Write session separator header:
    //       — "================================================================================"
    //       — "=== Session Start: {DateTime.Now:yyyy-MM-dd HH:mm:ss} =========================="
    //       — "================================================================================"
    //     — Hook AppDomain.CurrentDomain.ProcessExit += OnProcessExit
    //       — This is a fallback flush in case Shutdown() is never called (e.g., window X button)

    // FormatMessage(LogLevel level, string category, string message) → string
    //   — Get timestamp: DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
    //   — Get level label: s_levelLabels[(int)level]
    //   — Pad category to 10 characters: category.PadRight(10)
    //   — Return: "{timestamp} [{label}] {paddedCategory} | {message}"

    // OnProcessExit(object sender, EventArgs e)
    //   — Call Shutdown()
    //   — This is the fallback — ensures the file is flushed even if Core.UnloadContent() didn't run
