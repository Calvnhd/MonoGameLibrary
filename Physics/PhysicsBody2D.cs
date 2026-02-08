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
    /// Horizontal drag multiplier when grounded. Lower = more friction.
    /// </summary>
    public float GroundDrag { get; set; } = 0.85f;

    /// <summary>
    /// Horizontal drag multiplier when airborne. Higher = more air control.
    /// </summary>
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
        if (!IsOnGround)
        {
            Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * deltaTime);
        }

        if (Velocity.Y > MaxFallSpeed)
        {
            Velocity = new Vector2(Velocity.X, MaxFallSpeed);
        }

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
