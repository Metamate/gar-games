using GARCore;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mario8.Input;
using Mario8.LevelMaker;

namespace Mario8.States.GameStates;

public class StartState(Game1 game) : GameStateBase(game)
{
    private GameLevel _backgroundLevel;

    public override void Enter()
    {
        var levelMaker = new ComplexLevelMaker(Game.Content);
        _backgroundLevel = levelMaker.Generate(30, 9);
    }

    public override void Update(GameTime gameTime)
    {
        if (GameController.Start)
        {
            Game.SetState(new PlayState(Game));
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _backgroundLevel.Draw(spriteBatch, Game.ScreenScaleMatrix);

        spriteBatch.Begin(transformMatrix: Game.ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        TitleScreen.Draw(spriteBatch, Game1.TitleFont, Game1.DefaultFont, "Super Mario Bros", "Arrows: move   Space: jump",
            GameSettings.VirtualWidth, GameSettings.VirtualHeight);
        spriteBatch.End();
    }
}
