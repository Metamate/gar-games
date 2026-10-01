using GARCore.Graphics;

namespace Pokemon1;

// Bundles game-specific art and fonts so the rest of the codebase can access
// them through the service locator instead of mixing several global patterns.
public sealed class GameAssets
{
    public BitmapFont SmallFont { get; }
    public BitmapFont MediumFont { get; }
    public BitmapFont LargeFont { get; }

    public Tileset Tileset { get; }
    public TextureAtlas EntityAtlas { get; }

    public GameAssets(
        BitmapFont smallFont,
        BitmapFont mediumFont,
        BitmapFont largeFont,
        Tileset tileset,
        TextureAtlas entityAtlas)
    {
        SmallFont = smallFont;
        MediumFont = mediumFont;
        LargeFont = largeFont;
        Tileset = tileset;
        EntityAtlas = entityAtlas;
    }
}
