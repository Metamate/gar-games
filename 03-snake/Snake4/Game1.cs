using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake4;

public class Game1 : Core
{
    public const int VirtualWidth = 320;
    public const int VirtualHeight = 180;
    private Tilemap _tilemap;
    private AnimatedSprite _snake;
    private AnimatedSprite _bat;

    public Game1() : base("Snake", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        // The room is data too: the tilemap definition lists the tile for every cell.
        _tilemap = Tilemap.FromFile(Content, "images/tilemap-definition.xml");

        _snake = atlas.CreateAnimatedSprite("snake-animation");
        _bat = atlas.CreateAnimatedSprite("bat-animation");
    }

    protected override void Update(GameTime gameTime)
    {
        _snake.Update(gameTime);
        _bat.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        _tilemap.Draw(SpriteBatch);
        _snake.Draw(SpriteBatch, new Vector2(160, 80));
        _bat.Draw(SpriteBatch, new Vector2(200, 80));
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
