using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Birds4.GameStates;

// The bird is flying, and things are falling. Wait until everything has come to rest (or a
// few seconds have passed), then see how it went.
public class FlyState(Game1 game) : IState
{
    private const float MinSeconds = 1;
    private const float MaxSeconds = 7;
    private float _elapsed;

    public void Enter() => _elapsed = 0;
    public void Exit() { }

    public void Update(GameTime gameTime)
    {
        float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _elapsed += deltaSeconds;
        game.UpdateWorld(deltaSeconds);

        bool done = (_elapsed > MinSeconds && game.Physics.IsSettled) || _elapsed > MaxSeconds;
        if (!done)
            return;

        if (game.PigsLeft == 0)
        {
            game.Score += game.BirdsLeft * Game1.BirdBonus;
            game.LevelEndState.Won = true;
            game.ChangeState(game.LevelEndState);
        }
        else if (game.BirdsLeft > 0)
        {
            game.ChangeState(game.AimState);
        }
        else
        {
            game.LevelEndState.Won = false;
            game.ChangeState(game.LevelEndState);
        }
    }

    public void Draw(SpriteBatch spriteBatch) => game.DrawWorld();
}
