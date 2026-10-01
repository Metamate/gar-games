using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon2.Entities;

namespace Pokemon2.World;

// The overworld level: a small town on two tilemap layers (the ground, and tall grass over it) and the player entity.
public sealed class Level
{
    public Tilemap BaseLayer  { get; }
    public Tilemap GrassLayer { get; }

    public Player Player { get; }

    public Level(Player player, Tileset tileset)
    {
        Player     = player;
        BaseLayer  = new Tilemap(tileset, GameSettings.MapCols, GameSettings.MapRows);
        GrassLayer = new Tilemap(tileset, GameSettings.MapCols, GameSettings.MapRows);
        Town.Build(BaseLayer, GrassLayer);
    }

    // Whether an entity can't step onto this tile.
    public bool IsSolid(Point tile) => BaseLayer.GetTile(tile.X, tile.Y).IsSolid;

    public void Update(GameTime gameTime)
    {
        Player.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        BaseLayer.Draw(spriteBatch);
        GrassLayer.Draw(spriteBatch);
        Player.Draw(spriteBatch);
    }
}
