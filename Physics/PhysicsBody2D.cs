using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Physics;

/// <summary>
/// A 2D physics component that applies gravity, velocity, and drag to game objects.
/// </summary>
/// <remarks>
/// <para>
/// This component follows the composition pattern—game objects should own a 
/// <see cref="PhysicsBody2D"/> rather than inherit from a physics base class.
/// The component manages velocity but does not own position; the game object
/// is responsible for applying velocity to its position.
/// </para>
/// <example>
/// Basic usage in a game object:
/// <code>
/// public class Player
/// {
///     public Vector2 Position { get; set; }
///     public PhysicsBody2D Physics { get; } = new PhysicsBody2D();
///     
///     public void Update(float deltaTime)
///     {
///         // Apply physics (gravity, drag)
///         Physics.ApplyPhysics(deltaTime);
///         
///         // Move the object
///         Position += Physics.Velocity * deltaTime;
///         
///         // Collision system would set Physics.IsOnGround here
///     }
/// }
/// </code>
/// </example>
/// </remarks>
public class PhysicsBody2D
{
    /// <summary>
    /// Gets or sets the current velocity in pixels per second.
    /// </summary>
    /// <remarks>
    /// Positive X moves right, positive Y moves down (MonoGame coordinate system).
    /// Modify directly for input-driven movement, or use <see cref="ApplyImpulse"/>
    /// for physics-based forces like jumps.
    /// </remarks>
    public Vector2 Velocity { get; set; }

    /// <summary>
    /// Gets or sets the gravity acceleration in pixels per second squared.
    /// </summary>
    /// <remarks>
    /// Positive values pull downward. Typical platformer values range from 1500-3500.
    /// Higher values create a "heavier" feel; lower values feel "floaty."
    /// </remarks>
    public float Gravity { get; set; } = 2000f;

    /// <summary>
    /// Gets or sets the maximum fall speed (terminal velocity) in pixels per second.
    /// </summary>
    /// <remarks>
    /// Prevents objects from accelerating indefinitely when falling. Without this,
    /// long falls would result in extremely high velocities that can cause tunneling
    /// through platforms.
    /// </remarks>
    public float MaxFallSpeed { get; set; } = 600f;

    /// <summary>
    /// Gets or sets the horizontal velocity multiplier applied when grounded.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Applied each frame: <c>Velocity.X *= GroundDrag</c>.
    /// </para>
    /// <para>
    /// Values less than 1.0 slow the object down. Lower values = more friction.
    /// Typical values: 0.8-0.9 for responsive stopping, 0.5-0.7 for slippery surfaces.
    /// </para>
    /// </remarks>
    public float GroundDrag { get; set; } = 0.85f;

    /// <summary>
    /// Gets or sets the horizontal velocity multiplier applied when airborne.
    /// </summary>
    /// <remarks>
    /// Usually higher than <see cref="GroundDrag"/> to give players more air control.
    /// A value of 1.0 means no air drag (velocity is preserved).
    /// </remarks>
    public float AirDrag { get; set; } = 0.95f;

    /// <summary>
    /// Gets or sets whether this object is currently resting on the ground.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property should be set externally by the collision resolution system
    /// (e.g., <c>CollisionWorld</c>). Set to <c>true</c> when a downward collision
    /// is resolved; set to <c>false</c> at the start of each frame before collision
    /// checks.
    /// </para>
    /// <para>
    /// Affects gravity application (no gravity when grounded) and which drag value
    /// is used (<see cref="GroundDrag"/> vs <see cref="AirDrag"/>).
    /// </para>
    /// </remarks>
    public bool IsOnGround { get; set; }

    /// <summary>
    /// Applies physics simulation to the velocity.
    /// </summary>
    /// <param name="deltaTime">
    /// The elapsed time in seconds since the last frame.
    /// Typically obtained from <c>(float)gameTime.ElapsedGameTime.TotalSeconds</c>.
    /// </param>
    /// <remarks>
    /// <para>
    /// Call this method once per frame, before applying velocity to position.
    /// The method performs the following in order:
    /// </para>
    /// <list type="number">
    ///   <item>Applies gravity to Y velocity (if not grounded)</item>
    ///   <item>Clamps Y velocity to terminal velocity</item>
    ///   <item>Applies drag to X velocity</item>
    /// </list>
    /// <para>
    /// This method does NOT update position. The calling code should apply
    /// velocity to position: <c>Position += Physics.Velocity * deltaTime;</c>
    /// </para>
    /// </remarks>
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
    /// <param name="impulse">
    /// The velocity to add, in pixels per second.
    /// Use negative Y for upward impulses (jumps).
    /// </param>
    /// <remarks>
    /// <para>
    /// Use this method for physics-based forces like:
    /// </para>
    /// <list type="bullet">
    ///   <item>Jump initiation: <c>ApplyImpulse(new Vector2(0, -700))</c></item>
    ///   <item>Knockback: <c>ApplyImpulse(new Vector2(300, -100))</c></item>
    ///   <item>Explosion: <c>ApplyImpulse(directionFromExplosion * force)</c></item>
    /// </list>
    /// <example>
    /// <code>
    /// // Simple jump when space is pressed
    /// if (input.Keyboard.WasKeyPressed(Keys.Space) &amp;&amp; physics.IsOnGround)
    /// {
    ///     physics.ApplyImpulse(new Vector2(0, -600f));
    /// }
    /// </code>
    /// </example>
    /// </remarks>
    public void ApplyImpulse(Vector2 impulse)
    {
        Velocity += impulse;
    }
}
