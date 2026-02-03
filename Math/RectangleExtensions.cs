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
    /// <returns>
    /// The overlap depth on each axis. Negative values indicate rectA should move left/up.
    /// Returns <see cref="Vector2.Zero"/> if not intersecting.
    /// </returns>
    public static Vector2 GetIntersectionDepth(this Rectangle rectA, Rectangle rectB)
    {
        // Calculate half sizes
        float halfWidthA = rectA.Width * 0.5f;
        float halfHeightA = rectA.Height * 0.5f;
        float halfWidthB = rectB.Width * 0.5f;
        float halfHeightB = rectB.Height * 0.5f;

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
            rect.X + rect.Width * 0.5f,
            rect.Y + rect.Height * 0.5f
        );
    }
    /// <summary>
    /// Gets the position of the center of the bottom edge of the rectangle.
    /// </summary>
    public static Vector2 GetBottomCenter(this Rectangle rect)
    {
        return new Vector2(rect.X + rect.Width * 0.5f, rect.Bottom);
    }
}
