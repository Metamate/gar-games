using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Birds4.GameStates;

// A bird is in the slingshot: pull back and let go.
public class AimState(Game1 game) : IState
{
    public void Enter() => game.Slingshot.IsLoaded = true;
    public void Exit() => game.Slingshot.IsLoaded = false;

    public void Update(GameTime gameTime)
    {
        game.UpdateWorld((float)gameTime.ElapsedGameTime.TotalSeconds);
        if (game.Slingshot.Update(game.MousePosition(), out Vector2 position, out Vector2 velocity))
        {
            game.Launch(position, velocity);
            game.ChangeState(game.FlyState);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        game.DrawWorld();
        if (game.Slingshot.IsAiming)
            game.DrawTrajectory(game.Slingshot.AimPosition, game.Slingshot.AimVelocity);
    }
}
