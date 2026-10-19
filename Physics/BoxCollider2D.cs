using Microsoft.Xna.Framework;
using MonoGameLibrary.Math;

namespace MonoGameLibrary.Physics;

/// <summary>
/// An axis-aligned bounding box (AABB) collider for 2D collision detection.
/// </summary>
public class BoxCollider2D : ICollider2D
{
    /// <summary>
    /// Gets or sets the offset from the object's position to the collider's top-left corner.
    /// </summary>
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
