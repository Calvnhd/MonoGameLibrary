# MonoGameLibrary

Created by following the tutorial here: https://docs.monogame.net/articles/tutorials/building_2d_games/index.html

Separated out for re-useability and expanding upon

## Project Structure

MonoGameLibrary uses domain-based folders to organize code. The root contains only `Core.cs` (the main engine entry point).

| Folder | Purpose | Example additions |
|--------|---------|-------------------|
| Audio/ | Sound playback, music | SoundPool, MusicManager |
| Graphics/ | Sprites, textures, rendering | Particles, SpriteEffects |
| Input/ | Device input abstraction | Virtual gamepad, gestures |
| Math/ | Primitives, extensions, geometry | CircleF, segments, rays |
| Physics/ | Colliders, bodies, motion | CollisionWorld, triggers |
| Scenes/ | Scene lifecycle | Screen transitions |

### Growth Guidelines

> Create a new folder when you have **3+ related files** that form a cohesive domain. Don't pre-create folders for planned features.

This follows the "promote when patterns emerge" principle—avoid speculative architecture. When a folder exceeds 10 files, consider splitting it into sub-domains.

See [project structure research](../docs/research/monogamelibrary-project-structure.md) for detailed rationale.