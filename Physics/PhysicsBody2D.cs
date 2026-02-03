using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Physics;

/// <summary>
/// A 2D physics component that applies gravity, velocity, and drag to game objects.
/// </summary>
public class PhysicsBody2D
{
    /// <summary>
    /// Gets or sets the current velocity in pixels per second.
    /// </summary>
    public Vector2 Velocity { get; set; }

    /// <summary>
    /// Gets or sets the gravity acceleration in pixels per second squared.
    /// </summary>
    public float Gravity { get; set; } = 2000f;

    /// <summary>
    /// Gets or sets the maximum fall speed (terminal velocity) in pixels per second.
    /// </summary>
    public float MaxFallSpeed { get; set; } = 600f;

    /// <summary>
    /// Gets or sets the horizontal drag multiplier applied when grounded.
    /// </summary>
    /// <remarks>
    /// Only affects movement if using acceleration-based input. If you set
    /// horizontal velocity directly each frame, this value has no effect.
    /// </remarks>
    public float GroundDrag { get; set; } = 0.85f;

    /// <summary>
    /// Gets or sets the horizontal drag multiplier applied when airborne.
    /// </summary>
    /// <remarks>
    /// Controls how quickly horizontal momentum decays during jumps/falls.
    /// Lower values (e.g., 0.8) give less air control; higher values (e.g., 0.98)
    /// preserve momentum longer.
    /// </remarks>
    public float AirDrag { get; set; } = 0.95f;

    /// <summary>
    /// Gets or sets whether this object is currently resting on the ground.
    /// </summary>
    public bool IsOnGround { get; set; }

    /// <summary>
    /// Applies physics simulation (gravity, drag) to the velocity.
    /// </summary>
    /// <param name="deltaTime">Elapsed time in seconds since the last frame.</param>
    public void ApplyPhysics(float deltaTime)
    {
        // Apply gravity only when airborne
        // When grounded, we don't want gravity pushing us into the floor
        if (!IsOnGround)
        {
            Velocity = new Vector2(
                Velocity.X,
                Velocity.Y + Gravity * deltaTime
            );
        }

        // Clamp to terminal velocity to prevent infinite fall speed
        // This also prevents tunneling through thin platforms at high speeds
        if (Velocity.Y > MaxFallSpeed)
        {
            Velocity = new Vector2(Velocity.X, MaxFallSpeed);
        }

        // Apply horizontal drag (different values for ground vs air)
        // Ground drag is typically higher (lower value) for snappy stopping
        // Air drag is typically lower (higher value) for better air control
        float drag = IsOnGround ? GroundDrag : AirDrag;
        Velocity = new Vector2(Velocity.X * drag, Velocity.Y);
    }

    /// <summary>
    /// Applies an instantaneous velocity change (impulse).
    /// </summary>
    /// <param name="impulse">The velocity to add in pixels per second.</param>
    public void ApplyImpulse(Vector2 impulse)
    {
        Velocity += impulse;
    }
}
