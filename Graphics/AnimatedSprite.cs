using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics;

public class AnimatedSprite : Sprite
{
    private int _currentFrame;
    private TimeSpan _elapsed;
    private Animation _animation;
    private Dictionary<string, Animation> _animations = new();

    /// <summary>
    /// Gets or Sets the animation for this animated sprite.
    /// </summary>
    public Animation Animation
    {
        get => _animation;
        set
        {
            _animation = value;
            Region = _animation.Frames[0];
        }
    }

    /// <summary>
    /// Gets the name of the currently playing animation, or null if set directly via the Animation property.
    /// </summary>
    public string CurrentAnimationName { get; private set; }

    /// <summary>
    /// Creates a new animated sprite.
    /// </summary>
    public AnimatedSprite() { }

    /// <summary>
    /// Creates a new animated sprite with the specified frames and delay.
    /// </summary>
    /// <param name="animation">The animation for this animated sprite.</param>
    public AnimatedSprite(Animation animation)
    {
        Animation = animation;
    }

    /// <summary>
    /// Adds an animation with the specified name.
    /// </summary>
    /// <param name="name">The name to identify this animation.</param>
    /// <param name="animation">The animation to add.</param>
    public void AddAnimation(string name, Animation animation)
    {
        _animations[name] = animation;
    }

    /// <summary>
    /// Sets the current animation by name. If the animation is already playing, it will not restart.
    /// </summary>
    /// <param name="name">The name of the animation to play.</param>
    /// <returns>True if the animation was changed, false if already playing or not found.</returns>
    public bool SetAnimation(string name)
    {
        if (!_animations.TryGetValue(name, out var animation))
            return false;

        if (animation == _animation)
            return false;

        _animation = animation;
        _currentFrame = 0;
        _elapsed = TimeSpan.Zero;
        Region = _animation.Frames[0];
        CurrentAnimationName = name;
        return true;
    }
    /// <summary>
    /// Updates this animated sprite.
    /// </summary>
    /// <param name="gameTime">A snapshot of the game timing values provided by the framework.</param>
    public void Update(GameTime gameTime)
    {
        _elapsed += gameTime.ElapsedGameTime;

        if (_elapsed >= _animation.Delay)
        {
            _elapsed -= _animation.Delay;
            _currentFrame++;

            if (_currentFrame >= _animation.Frames.Count)
            {
                _currentFrame = 0;
            }

            Region = _animation.Frames[_currentFrame];
        }
    }


}
