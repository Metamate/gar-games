using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer1.LevelMaker;

public class GameLevel(Tilemap tilemap, Tilemap toppers, TextureRegion background)
{
    public Tilemap Tilemap { get; } = tilemap;
    public Tilemap Toppers { get; } = toppers;
    public TextureRegion Background { get; set; } = background;

    public void RandomizeGraphics(LevelMakerBase maker)
    {
        Tilemap.Tileset = maker.GetRandomTileset();
        Toppers.Tileset = maker.GetRandomTopperset();
        Background = maker.GetRandomBackground();
    }

    public void Draw(SpriteBatch spriteBatch, Matrix screenScale)
    {
        spriteBatch.Begin(transformMatrix: screenScale, samplerState: SamplerState.PointClamp);
        Background.Draw(spriteBatch, Vector2.Zero, Color.White);
        Tilemap.Draw(spriteBatch);
        Toppers.Draw(spriteBatch);
        spriteBatch.End();
    }
}
