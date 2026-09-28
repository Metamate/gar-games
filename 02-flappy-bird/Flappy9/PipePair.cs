using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy9;

public class PipePair
{
    private const int PipeSpeed = 60;
    public const int PipeGap = 90;
    private float _x;
    public Pipe TopPipe { get; set; }
    public Pipe BottomPipe { get; set; }
    public bool Remove { get; set; }

    public PipePair(float y)
    {
        _x = Game1.VirtualWidth;
        TopPipe = new Pipe(new Vector2(_x, y - Art.Pipe.Height), true);
        BottomPipe = new Pipe(new Vector2(_x, y + PipeGap), false);
    }

    public void Update(GameTime gameTime)
    {
        if (_x > -Art.Pipe.Width)
        {
            _x -= PipeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            TopPipe.Position = new Vector2(_x, TopPipe.Position.Y);
            BottomPipe.Position = new Vector2(_x, BottomPipe.Position.Y);
        }
        else
        {
            Remove = true;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        TopPipe.Draw(spriteBatch);
        BottomPipe.Draw(spriteBatch);
    }
}