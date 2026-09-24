using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy7;

public class PipePair
{
    private const int PipeSpeed = 60;
    public const int PipeGap = 90;
    private Pipe _topPipe;
    private Pipe _bottomPipe;
    private float _x;
    public bool Remove { get; set; }

    public PipePair(float y)
    {
        _x = Game1.VirtualWidth;
        _topPipe = new Pipe(new Vector2(_x, y - Art.Pipe.Height), true);
        _bottomPipe = new Pipe(new Vector2(_x, y + PipeGap), false);
    }

    public void Update(GameTime gameTime)
    {
        if (_x > -Art.Pipe.Width)
        {
            _x -= PipeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _topPipe.Position = new Vector2(_x, _topPipe.Position.Y);
            _bottomPipe.Position = new Vector2(_x, _bottomPipe.Position.Y);
        }
        else
        {
            Remove = true;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _topPipe.Draw(spriteBatch);
        _bottomPipe.Draw(spriteBatch);
    }
}