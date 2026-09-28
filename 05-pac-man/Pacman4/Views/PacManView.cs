using System;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman4.Views;

// Draws Pac-Man: one chomping animation, facing right, turned to where he's going.
public class PacManView
{
    private readonly AnimatedSprite _chomp;
    private readonly Animation _death;

    public PacManView(TextureAtlas atlas)
    {
        _chomp = atlas.CreateAnimatedSprite("pacman-chomp");
        _chomp.CenterOrigin();
        _death = atlas.GetAnimation("pacman-death");
    }

    // He only chomps while he moves.
    public void Update(GameTime gameTime, PacMan pacMan)
    {
        if (pacMan.Heading != Direction.None)
            _chomp.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, PacMan pacMan)
    {
        _chomp.Rotation = MathF.Atan2(pacMan.Facing.Y, pacMan.Facing.X);
        _chomp.Draw(spriteBatch, pacMan.Position);
    }

    // The death animation plays once, with Pac-Man facing up.
    public void DrawDying(SpriteBatch spriteBatch, PacMan pacMan, float seconds)
    {
        var frames = _death.Frames;
        int frame = Math.Min((int)(seconds / _death.Delay.TotalSeconds), frames.Count - 1);
        var sprite = new Sprite(frames[frame]) { Rotation = -MathHelper.PiOver2 };
        sprite.CenterOrigin();
        sprite.Draw(spriteBatch, pacMan.Position);
    }
}
