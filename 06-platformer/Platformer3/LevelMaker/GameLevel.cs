using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer3.Entities;
using Platformer3.Input;

namespace Platformer3.LevelMaker;

public class GameLevel(Tilemap tilemap, Tilemap toppers, TextureRegion background)
{
    public Tilemap Tilemap { get; } = tilemap;
    public Tilemap Toppers { get; } = toppers;
    public TextureRegion Background { get; set; } = background;
    public Player Player { get; set; }

    public void RandomizeGraphics(LevelMakerBase maker)
    {
        Tilemap.Tileset = maker.GetRandomTileset();
        Toppers.Tileset = maker.GetRandomTopperset();
        Background = maker.GetRandomBackground();
    }

    public void Update(GameTime gameTime)
    {
        if (GameController.ToggleDebug)
        {
            DebugDraw.Enabled = !DebugDraw.Enabled;
        }

        Player?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, Matrix screenScale)
    {
        spriteBatch.Begin(transformMatrix: screenScale, samplerState: SamplerState.PointClamp);
        Background.Draw(spriteBatch, Vector2.Zero, Color.White);
        Tilemap.Draw(spriteBatch);
        Toppers.Draw(spriteBatch);
        Player?.Draw(spriteBatch);
        DrawDebug(spriteBatch);
        spriteBatch.End();
    }

    // With debug drawing on (F1), outline the solid tiles and the player's hitbox.
    private void DrawDebug(SpriteBatch spriteBatch)
    {
        for (int row = 0; row < Tilemap.Rows; row++)
        {
            for (int column = 0; column < Tilemap.Columns; column++)
            {
                if (Tilemap.GetTile(column, row).IsSolid)
                {
                    Vector2 position = Tilemap.TileToPoint(column, row);
                    DebugDraw.Rectangle(spriteBatch, new Rectangle((int)position.X, (int)position.Y, (int)Tilemap.TileWidth, (int)Tilemap.TileHeight), Color.Red);
                }
            }
        }

        if (Player != null)
        {
            DebugDraw.Rectangle(spriteBatch, Player.Bounds, Color.Lime);
        }
    }
}
