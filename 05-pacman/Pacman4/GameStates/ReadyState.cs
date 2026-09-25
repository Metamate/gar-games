using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman4.GameStates;

// "READY!": a short pause before play starts, after every life lost and every new level.
public class ReadyState(Game1 game) : IState
{
    private const float Seconds = 2;
    private float _elapsed;

    public void Enter() => _elapsed = 0;
    public void Exit() { }

    public void Update(GameTime gameTime)
    {
        _elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_elapsed >= Seconds)
            game.ChangeState(game.PlayState);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        game.DrawWorld();
        game.DrawHud();
        game.DrawMessage("READY!", Color.Yellow);
    }
}
