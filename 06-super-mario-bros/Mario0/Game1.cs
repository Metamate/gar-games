using System;
using System.Collections.Generic;
using GARCore;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mario0;

public class Game1 : Core
{
    private const int TileSize = 18;
    private const int Columns = GameSettings.VirtualWidth / TileSize;
    private const int Rows = GameSettings.VirtualHeight / TileSize;
    private const int GroundHeight = 3;
    private const int GroundTile = 0;

    private readonly List<Tileset> _tilesets = [];
    private Tilemap _tilemap;

    public Game1() : base("Super Mario Bros", 1152, 648, GameSettings.VirtualWidth, GameSettings.VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        // tiles.png holds four tilesets, one under the other. Each is a row of 18x18 tiles in
        // its own colours, so the same level can be drawn in four different styles.
        Texture2D texture = Content.Load<Texture2D>("images/tiles");
        int tilesetHeight = texture.Height / 4;

        for (int i = 0; i < 4; i++)
        {
            _tilesets.Add(new Tileset(new TextureRegion(texture, 0, i * tilesetHeight, texture.Width, tilesetHeight), TileSize, TileSize));
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
