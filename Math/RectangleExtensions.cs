using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Math;

/// <summary>
/// Extension methods for the <see cref="Rectangle"/> struct providing
/// AABB (Axis-Aligned Bounding Box) collision utilities.
/// </summary>
public static class RectangleExtensions
{
    /// <summary>
    /// Calculates the signed depth of intersection between two rectangles.
    /// </summary>
    /// <param name="rectA">The first rectangle.</param>
    /// <param name="rectB">The second rectangle to check against.</param>
    /// <returns>
    /// A <see cref="Vector2"/> representing the overlap depth on each axis.
    /// The X component is negative if <paramref name="rectA"/> is to the left of <paramref name="rectB"/>.
    /// The Y component is negative if <paramref name="rectA"/> is above <paramref name="rectB"/>.
    /// Returns <see cref="Vector2.Zero"/> if the rectangles do not intersect.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The returned depth vector indicates how far to move <paramref name="rectA"/> to resolve
    /// the collision. To properly separate overlapping objects, move along the axis with the
    /// smaller absolute value (the shallower penetration).
    /// </para>
    /// <example>
    /// <code>
    /// Rectangle player = new Rectangle(100, 100, 32, 32);
    /// Rectangle platform = new Rectangle(90, 120, 64, 16);
    /// 
    /// Vector2 depth = player.GetIntersectionDepth(platform);
    /// 
    /// if (depth != Vector2.Zero)
    /// {
    ///     // Resolve along the shallower axis
    ///     if (Math.Abs(depth.X) &lt; Math.Abs(depth.Y))
    ///     {
    ///         playerPosition.X += depth.X; // Push horizontally
    ///     }
    ///     else
    ///     {
    ///         playerPosition.Y += depth.Y; // Push vertically
    ///         if (depth.Y &lt; 0)
    ///             isOnGround = true; // Landed on top of platform
    ///     }
    /// }
    /// </code>
    /// </example>
    /// </remarks>
    public static Vector2 GetIntersectionDepth(this Rectangle rectA, Rectangle rectB)
    {
        // Calculate half sizes
        float halfWidthA = rectA.Width / 2.0f;
        float halfHeightA = rectA.Height / 2.0f;
        float halfWidthB = rectB.Width / 2.0f;
        float halfHeightB = rectB.Height / 2.0f;

        // Calculate centers
        float centerAX = rectA.Left + halfWidthA;
        float centerAY = rectA.Top + halfHeightA;
        float centerBX = rectB.Left + halfWidthB;
        float centerBY = rectB.Top + halfHeightB;

        // Calculate distance between centers
        float distanceX = centerAX - centerBX;
        float distanceY = centerAY - centerBY;

        // Calculate minimum non-intersecting distance (sum of half-sizes)
        float minDistanceX = halfWidthA + halfWidthB;
        float minDistanceY = halfHeightA + halfHeightB;

        // If not intersecting on either axis, return zero
        if (System.Math.Abs(distanceX) >= minDistanceX || System.Math.Abs(distanceY) >= minDistanceY)
        {
            return Vector2.Zero;
        }

        // Calculate overlap depth on each axis
        // The sign indicates direction: negative = rectA should move left/up
        float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
        float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;

        return new Vector2(depthX, depthY);
    }

    /// <summary>
    /// Gets the center point of the rectangle.
    /// </summary>
    /// <param name="rect">The rectangle.</param>
    /// <returns>
    /// A <see cref="Vector2"/> representing the center coordinates of the rectangle.
    /// </returns>
    /// <remarks>
    /// Unlike <see cref="Rectangle.Center"/> which returns a <see cref="Point"/> (integers),
    /// this method returns a <see cref="Vector2"/> for precision in physics calculations.
    /// </remarks>
    public static Vector2 GetCenter(this Rectangle rect)
    {
        return new Vector2(
            rect.X + rect.Width / 2.0f,
            rect.Y + rect.Height / 2.0f
        );
    }
}
