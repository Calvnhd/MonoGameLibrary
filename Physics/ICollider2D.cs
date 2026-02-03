using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Physics;

/// <summary>
/// Interface for 2D collision shapes that can be attached to game objects.
/// </summary>
/// <remarks>
/// <para>
/// Colliders define a shape and can calculate their world-space bounds given a position.
/// They don't perform collision detection themselves—that responsibility belongs to
/// a collision system like <c>CollisionWorld</c>.
/// </para>
/// <para>
/// Implementations include <see cref="BoxCollider2D"/> for axis-aligned bounding boxes.
/// Future implementations could include circles, capsules, or polygons.
/// </para>
/// </remarks>
public interface ICollider2D
{
    /// <summary>
    /// Calculates the world-space bounds of this collider.
    /// </summary>
    /// <param name="position">The position of the owning object.</param>
    /// <returns>A <see cref="Rectangle"/> representing the collider's world-space bounds.</returns>
    /// <remarks>
    /// The returned bounds account for any offset configured on the collider,
    /// allowing hitboxes that don't align with the object's origin.
    /// </remarks>
    Rectangle GetBounds(Vector2 position);

    /// <summary>
    /// Checks whether this collider intersects with a rectangle.
    /// </summary>
    /// <param name="position">The position of the owning object.</param>
    /// <param name="other">The rectangle to check against.</param>
    /// <returns><c>true</c> if the collider overlaps with the rectangle; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// Use this for quick overlap checks against static level geometry.
    /// For collision resolution, use <see cref="GetIntersectionDepth"/> instead.
    /// </remarks>
    bool Intersects(Vector2 position, Rectangle other);

    /// <summary>
    /// Calculates the signed intersection depth between this collider and a rectangle.
    /// </summary>
    /// <param name="position">The position of the owning object.</param>
    /// <param name="other">The rectangle to check against.</param>
    /// <returns>
    /// A <see cref="Vector2"/> representing the overlap depth on each axis.
    /// Returns <see cref="Vector2.Zero"/> if not intersecting.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The returned depth indicates how far to move the object to resolve the collision.
    /// Resolve along the axis with the smaller absolute value (shallower penetration).
    /// </para>
    /// <para>
    /// Sign convention: negative X means move left, negative Y means move up.
    /// </para>
    /// </remarks>
    Vector2 GetIntersectionDepth(Vector2 position, Rectangle other);
}
