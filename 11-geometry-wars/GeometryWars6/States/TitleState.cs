using GARCore.Graphics;
using GARCore.States;
using GeometryWars6.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryWars6.States;

// The title screen: Enter swaps it for a new game, as the game over screen does.
public sealed class TitleState : GameStateBase
{
    private readonly Game1 _game;
    private readonly PlayContext _context;

    public TitleState(Game1 game, PlayContext context)
    {
        _game = game;
        _context = context;
    }

    // The game behind the title: a play state that is drawn, but never updated.
    private PlayState _preview;

    public override void Enter()
    {
        _preview = new PlayState(_game, _context);
        _preview.Enter();
    }

    public override void Exit() => _preview.Exit();

    public override void Draw(SpriteBatch spriteBatch) => _preview.Draw(spriteBatch);

    public override void Update(GameTime gameTime)
    {
        if (_context.Controller.WasConfirmPressed)
        {
            _game.StateStack.Pop();
            _game.StateStack.Push(new PlayState(_game, _context));
        }
    }

    public override void DrawHUD(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        TitleScreen.Draw(spriteBatch, _context.Assets.NameFont, _context.Assets.TitleFont, "Geometry Wars", "WASD: move   Mouse: aim and fire",
            (int)_context.Frame.ScreenSize.X, (int)_context.Frame.ScreenSize.Y);
        spriteBatch.End();
    }
}
