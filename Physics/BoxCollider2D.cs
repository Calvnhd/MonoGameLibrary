using Microsoft.Xna.Framework;
using MonoGameLibrary.Math;

namespace MonoGameLibrary.Physics;

/// <summary>
/// An axis-aligned bounding box (AABB) collider for 2D collision detection.
/// </summary>
/// <remarks>
/// <para>
/// Box colliders define a rectangular collision area that can be positioned with an
/// offset from the owning object's origin. This allows hitboxes that differ from
/// the visual sprite bounds.
/// </para>
/// <example>
/// Creating a collider smaller than the sprite:
/// <code>
/// // Sprite is 64x64, but we want a 32x48 hitbox centered at the bottom
/// var collider = new BoxCollider2D
/// {
///     Width = 32,
///     Height = 48,
///     Offset = new Vector2(16, 16)  // Shift right and down to center
/// };
/// 
/// // Get world bounds for collision checks
/// Rectangle bounds = collider.GetBounds(playerPosition);
/// </code>
/// </example>
/// <example>
/// Using with collision resolution:
/// <code>
/// // Check collision against a platform
/// Vector2 depth = collider.GetIntersectionDepth(position, platform);
/// 
/// if (depth != Vector2.Zero)
/// {
///     // Resolve along the shallower axis
///     if (Math.Abs(depth.X) &lt; Math.Abs(depth.Y))
///         position.X += depth.X;
///     else
///         position.Y += depth.Y;
/// }
/// </code>
/// </example>
/// </remarks>
public class BoxCollider2D : ICollider2D
{
    /// <summary>
    /// Gets or sets the offset from the object's position to the collider's top-left corner.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use offset to position the hitbox relative to the sprite. Common patterns:
    /// </para>
    /// <list type="bullet">
    ///   <item><c>Vector2.Zero</c> - Collider aligns with sprite top-left</item>
    ///   <item><c>new Vector2(16, 0)</c> - Collider is 16 pixels right of sprite</item>
    ///   <item><c>new Vector2(-Width/2, -Height/2)</c> - Center-origin positioning</item>
    /// </list>
    /// </remarks>
    public Vector2 Offset { get; set; }

    /// <summary>
    /// Gets or sets the width of the collision box in pixels.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the collision box in pixels.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Creates a new box collider with default values (0 size, no offset).
    /// </summary>
    public BoxCollider2D()
    {
    }

    /// <summary>
    /// Creates a new box collider with the specified dimensions.
    /// </summary>
    /// <param name="width">The width of the collision box in pixels.</param>
    /// <param name="height">The height of the collision box in pixels.</param>
    public BoxCollider2D(int width, int height)
    {
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Creates a new box collider with the specified dimensions and offset.
    /// </summary>
    /// <param name="width">The width of the collision box in pixels.</param>
    /// <param name="height">The height of the collision box in pixels.</param>
    /// <param name="offset">The offset from object origin to collider top-left.</param>
    public BoxCollider2D(int width, int height, Vector2 offset)
    {
        Width = width;
        Height = height;
        Offset = offset;
    }

    /// <inheritdoc />
    public Rectangle GetBounds(Vector2 position)
    {
        return new Rectangle(
            (int)(position.X + Offset.X),
            (int)(position.Y + Offset.Y),
            Width,
            Height
        );
    }

    /// <inheritdoc />
    public bool Intersects(Vector2 position, Rectangle other)
    {
        return GetBounds(position).Intersects(other);
    }

    /// <inheritdoc />
    public Vector2 GetIntersectionDepth(Vector2 position, Rectangle other)
    {
        return GetBounds(position).GetIntersectionDepth(other);
    }
}
