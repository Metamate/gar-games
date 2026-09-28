using System.Collections.Generic;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman4.Views;

// Draws the ghosts. A ghost is a body and a pair of eyes that look where it's going. The view
// only asks the ghost how it looks (GhostLook); it doesn't know about modes or states.
public class GhostView
{
    private readonly Dictionary<string, AnimatedSprite> _bodies = [];
    private readonly AnimatedSprite _frightened;
    private readonly AnimatedSprite _flashing;
    private readonly Dictionary<Point, Sprite> _eyes = [];

    public GhostView(TextureAtlas atlas)
    {
        foreach (string name in new[] { "blinky", "pinky", "inky", "clyde" })
            _bodies[name] = Centered(atlas.CreateAnimatedSprite($"ghost-{name}"));
        _frightened = Centered(atlas.CreateAnimatedSprite("ghost-frightened"));
        _flashing = Centered(atlas.CreateAnimatedSprite("ghost-flashing"));

        _eyes[Direction.Up] = Centered(atlas.CreateSprite("eyes-up"));
        _eyes[Direction.Down] = Centered(atlas.CreateSprite("eyes-down"));
        _eyes[Direction.Left] = Centered(atlas.CreateSprite("eyes-left"));
        _eyes[Direction.Right] = Centered(atlas.CreateSprite("eyes-right"));
    }

    public void Update(GameTime gameTime)
    {
        foreach (AnimatedSprite body in _bodies.Values)
            body.Update(gameTime);
        _frightened.Update(gameTime);
        _flashing.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, Ghost ghost)
    {
        switch (ghost.Look)
        {
            case GhostLook.Normal:
                _bodies[ghost.Name].Draw(spriteBatch, ghost.Position);
                _eyes[ghost.Facing].Draw(spriteBatch, ghost.Position);
                break;
            case GhostLook.Frightened:
                _frightened.Draw(spriteBatch, ghost.Position);
                break;
            case GhostLook.FrightenedEnding:
                _flashing.Draw(spriteBatch, ghost.Position);
                break;
            case GhostLook.Eyes:
                _eyes[ghost.Facing].Draw(spriteBatch, ghost.Position);
                break;
        }
    }

    private static T Centered<T>(T sprite) where T : Sprite
    {
        sprite.CenterOrigin();
        return sprite;
    }
}
