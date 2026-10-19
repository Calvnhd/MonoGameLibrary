namespace MonoGameLibrary.Utilities;

/// <summary>
/// Predefined log category constants for discoverability via autocomplete.
/// Custom strings like "Player" or "AI" are always accepted — these constants are convenience, not a constraint.
/// </summary>
public static class LogCategory
{
    /// <summary>Game lifecycle: initialization, shutdown, scene transitions.</summary>
    public const string Core = "Core";

    /// <summary>Physics bodies, colliders, collision resolution.</summary>
    public const string Physics = "Physics";

    /// <summary>Sprites, animations, textures, rendering.</summary>
    public const string Graphics = "Graphics";

    /// <summary>Keyboard, mouse, gamepad input handling.</summary>
    public const string Input = "Input";
}
