using System;
using System.Collections.Generic;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer0;

public class Game1 : Core
{
    private const int TileSize = 16;
    private const int Columns = GameSettings.VirtualWidth / TileSize;
    private const int Rows = GameSettings.VirtualHeight / TileSize;
    private const int GroundHeight = 3;
    private const int GroundTile = 12;

    private readonly List<Tileset> _tilesets = [];
    private Tilemap _tilemap;

    public Game1() : base("Platformer", 1280, 720, GameSettings.VirtualWidth, GameSettings.VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        // tiles.png is a 6x10 grid of tilesets. Each tileset is a small sheet of 16x16 tiles
        // in its own colour scheme, so the same level can be drawn in 60 different styles.
        Texture2D texture = Content.Load<Texture2D>("images/tiles");
        int tilesetWidth = texture.Width / 6;
        int tilesetHeight = texture.Height / 10;

        for (int i = 0; i < 6 * 10; i++)
        {
            int x = i % 6 * tilesetWidth;
            int y = i / 6 * tilesetHeight;
            _tilesets.Add(new Tileset(new TextureRegion(texture, x, y, tilesetWidth, tilesetHeight), TileSize, TileSize));
        }

        GenerateLevel();
    }

    // The level is generated in code instead of loaded from a file:
    // empty sky, with a few rows of solid ground at the bottom.
    private void GenerateLevel()
    {
        Tileset tileset = _tilesets[Random.Shared.Next(_tilesets.Count)];
        _tilemap = new Tilemap(tileset, Columns, Rows);

        for (int x = 0; x < Columns; x++)
        {
            for (int y = Rows - GroundHeight; y < Rows; y++)
            {
                // A tile is more than a graphic: it also knows whether it is solid.
                _tilemap.SetTile(x, y, new Tile(GroundTile, isSolid: true));
            }
        }
    }

    protected override void Update(GameTime gameTime)
    {
        // Press R to generate the level again with a random tileset.
        if (Input.Keyboard.WasKeyJustPressed(Keys.R))
        {
            GenerateLevel();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        _tilemap.Draw(SpriteBatch);
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
