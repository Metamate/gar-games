using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz4.GameStates;

public class PlayState(Game1 game) : IState
{
    public void Enter() { }
    public void Exit() { }

    public void Update(GameTime gameTime)
    {
        game.UpdatePlay((float)gameTime.ElapsedGameTime.TotalSeconds);

        if (game.World.ZombieReachedHouse)
        {
            game.EndState.Won = false;
            game.ChangeState(game.EndState);
        }
        else if (game.AllZombiesSpawned && game.World.ZombieCount == 0)
        {
            game.EndState.Won = true;
            game.ChangeState(game.EndState);
        }
    }

    public void Draw(SpriteBatch spriteBatch) => game.DrawGame(showCursor: true);
}
