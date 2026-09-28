using System;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman0.Views;

// Draws Pac-Man: one chomping animation, facing right, turned to where he's going.
public class PacManView
{
    private readonly AnimatedSprite _chomp;

    public PacManView(TextureAtlas atlas)
    {
        _chomp = atlas.CreateAnimatedSprite("pacman-chomp");
        _chomp.CenterOrigin();
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
}
