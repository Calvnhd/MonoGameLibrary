using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Physics;

/// <summary>
/// Interface for 2D collision shapes that can be attached to game objects.
/// </summary>
public interface ICollider2D
{
    /// <summary>
    /// Calculates the world-space bounds of this collider.
    /// </summary>
    Rectangle GetBounds(Vector2 position);

    /// <summary>
    /// Checks whether this collider intersects with a rectangle.
    /// </summary>
    bool Intersects(Vector2 position, Rectangle other);

    /// <summary>
    /// Calculates the signed intersection depth between this collider and a rectangle.
    /// </summary>
    /// <returns>
    /// The overlap depth on each axis. Returns <see cref="Vector2.Zero"/> if not intersecting.
    /// </returns>
    Vector2 GetIntersectionDepth(Vector2 position, Rectangle other);
}
