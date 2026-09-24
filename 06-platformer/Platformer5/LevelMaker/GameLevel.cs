using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer5.Entities;
using Platformer5.Input;
using Platformer5.Graphics;

namespace Platformer5.LevelMaker;

public class GameLevel(Tilemap tilemap, Tilemap toppers, TextureRegion background)
{
    public Tilemap Tilemap { get; } = tilemap;
    public Tilemap Toppers { get; } = toppers;
    public TextureRegion Background { get; set; } = background;
    public Camera Camera { get; } = new();
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
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        if (Player == null) return;

        float worldWidth = Tilemap.Columns * Tilemap.TileWidth;
        float playerCenterX = Player.Position.X + (Player.Sprite.Width / 2f);
        float clampedX = MathHelper.Clamp(playerCenterX, GameSettings.VirtualWidth / 2f, worldWidth - GameSettings.VirtualWidth / 2f);

        Camera.Follow(
            new Vector2(clampedX, GameSettings.VirtualHeight / 2f),
            GameSettings.VirtualWidth,
            GameSettings.VirtualHeight
        );
    }

    public void Draw(SpriteBatch spriteBatch, Matrix screenScale)
    {
        DrawBackground(spriteBatch, screenScale);

        spriteBatch.Begin(transformMatrix: Camera.Transform * screenScale, samplerState: SamplerState.PointClamp);
        Tilemap.Draw(spriteBatch);
        Toppers.Draw(spriteBatch);
        Player?.Draw(spriteBatch);
        DrawDebug(spriteBatch);
        spriteBatch.End();
    }

    public void DrawBackground(SpriteBatch spriteBatch, Matrix screenScale)
    {
        float parallaxFactor = 0.5f;
        float bgOffset = -(Camera.Position.X * parallaxFactor) % Background.Width;

        spriteBatch.Begin(transformMatrix: screenScale, samplerState: SamplerState.PointClamp);
        Background.Draw(spriteBatch, new Vector2(bgOffset, 0), Color.White);
        Background.Draw(spriteBatch, new Vector2(bgOffset + Background.Width, 0), Color.White);
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
