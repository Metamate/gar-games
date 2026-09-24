using System;
using System.Collections.Generic;
using System.Linq;
using GMDCore.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer6.LevelMaker;

public abstract class LevelMakerBase
{
    private const int TileSize = 16;
    private const int TilesetsColumns = 6;
    private const int TilesetsRows = 10;
    private const int ToppersetsColumns = 6;
    private const int ToppersetsRows = 18;

    private readonly ContentManager _content;

    public List<Tileset> Tilesets { get; } = [];
    public List<Tileset> Toppersets { get; } = [];
    public List<TextureRegion> Backgrounds { get; } = [];
    public List<TextureRegion> Bushes { get; } = [];
    public List<TextureRegion> Gems { get; } = [];
    public List<TextureRegion> MysteryBoxes { get; } = [];
    protected Tilemap Tilemap { get; set; }
    protected Tilemap Toppers { get; set; }

    public LevelMakerBase(ContentManager content)
    {
        _content = content;
        Tilesets = CreateTilesetsFromFile("images/tiles", TilesetsColumns, TilesetsRows, TileSize);
        Toppersets = CreateTilesetsFromFile("images/tile_tops", ToppersetsColumns, ToppersetsRows, TileSize);
        Backgrounds = GetTextureRegionsFromFile("images/backgrounds", 1, 3);
        Bushes = GetTextureRegionsFromFile("images/bushes_and_cacti", 7, 5)
            .Where((_, i) => new[] { 0, 1, 4, 5, 6 }.Contains(i % 7)).ToList(); // we only want graphics 0, 1, 4, 5, 6 per row
        Gems = GetTextureRegionsFromFile("images/gems", 4, 2);
        MysteryBoxes = GetTextureRegionsFromFile("images/jump_blocks", 6, 5);
    }

    public abstract GameLevel Generate(int columns, int rows);

    public Tileset GetRandomTileset() => GetRandom(Tilesets);
    public Tileset GetRandomTopperset() => GetRandom(Toppersets);
    public TextureRegion GetRandomBackground() => GetRandom(Backgrounds);
    public TextureRegion GetRandomBush() => GetRandom(Bushes);
    public TextureRegion GetRandomGem() => GetRandom(Gems);
    public TextureRegion GetRandomMysteryBox() => GetRandom(MysteryBoxes);

    public List<TextureRegion> GetTextureRegionsFromFile(string file, int columns, int rows)
    {
        List<TextureRegion> textureRegions = [];
        Texture2D texture = _content.Load<Texture2D>(file);

        int textureRegionWidth = texture.Width / columns;
        int textureRegionHeight = texture.Height / rows;
        int count = columns * rows;

        for (int i = 0; i < count; i++)
        {
            int x = i % columns * textureRegionWidth;
            int y = i / columns * textureRegionHeight;

            TextureRegion textureRegion = new(texture, x, y, textureRegionWidth, textureRegionHeight);
            textureRegions.Add(textureRegion);
        }

        return textureRegions;
    }

    protected void CreateGroundColumn(int x, int groundHeight)
    {
        for (int y = 0; y < Tilemap.Rows; y++)
        {
            if (y >= Tilemap.Rows - groundHeight)
            {
                if (y == Tilemap.Rows - groundHeight)
                {
                    Tilemap.SetTile(x, y, new Tile(12, true));
                    Toppers.SetTile(x, y, new Tile(0));
                }
                else
                {
                    Tilemap.SetTile(x, y, new Tile(12, true));
                }
            }
        }
    }

    private List<Tileset> CreateTilesetsFromFile(string file, int columns, int rows, int tileSize)
    {
        List<Tileset> tilesets = [];
        Texture2D texture = _content.Load<Texture2D>(file);

        int tilesetWidth = texture.Width / columns;
        int tilesetHeight = texture.Height / rows;
        int count = columns * rows;

        for (int i = 0; i < count; i++)
        {
            int x = i % columns * tilesetWidth;
            int y = i / columns * tilesetHeight;

            TextureRegion textureRegion = new(texture, x, y, tilesetWidth, tilesetHeight);
            Tileset tileset = new(textureRegion, tileSize, tileSize);
            tilesets.Add(tileset);
        }

        return tilesets;
    }

    private static T GetRandom<T>(List<T> list) => list[Random.Shared.Next(list.Count)];
}
