using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GMDCore.Graphics;

// A pixel-perfect bitmap font backed by a pre-generated glyph atlas.
// The atlas is rendered without anti-aliasing (1-bit per pixel) so glyphs stay
// crisp when the render-target is upscaled with point filtering.
// 
// Atlas layout: printable ASCII 32–126 (95 chars), 16 columns per row,
// each character occupies a cell of CellW × CellH pixels.
// Per-character advance widths are stored separately, so a variable-width font works too.
// The atlases are made from Press Start 2P (Tools/GenerateFontAtlas.py), which is monospaced.
public sealed class BitmapFont
{
    private readonly Texture2D _atlas;
    private readonly int       _cellW;
    private readonly int       _cellH;
    private readonly int[]     _advances;   // advance width per character (index = codepoint - 32)

    private const int FirstChar = 32;
    private const int Cols      = 16;

    public int LineHeight => _cellH;

    private static readonly int[] AdvancesSmall  = Enumerable.Repeat(8, 95).ToArray();
    private static readonly int[] AdvancesMedium = Enumerable.Repeat(16, 95).ToArray();
    private static readonly int[] AdvancesLarge  = Enumerable.Repeat(32, 95).ToArray();

    public static BitmapFont CreateSmall(Texture2D atlas)  => new(atlas, 8,  8,  AdvancesSmall);
    public static BitmapFont CreateMedium(Texture2D atlas) => new(atlas, 16, 16, AdvancesMedium);
    public static BitmapFont CreateLarge(Texture2D atlas)  => new(atlas, 32, 32, AdvancesLarge);

    private BitmapFont(Texture2D atlas, int cellW, int cellH, int[] advances)
    {
        _atlas    = atlas;
        _cellW    = cellW;
        _cellH    = cellH;
        _advances = advances;
    }

    // Draw a string at the given position.
    public void Draw(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
    {
        float x = position.X, startX = position.X, y = position.Y;
        foreach (char c in text)
        {
            if (c == '\n') { x = startX; y += _cellH + 1; continue; }
            int idx = c - FirstChar;
            if (idx < 0 || idx >= _advances.Length) { x += _advances[0]; continue; }
            int col = idx % Cols;
            int row = idx / Cols;
            spriteBatch.Draw(_atlas, new Vector2(x, y),
                new Rectangle(col * _cellW, row * _cellH, _cellW, _cellH), color);
            x += _advances[idx];
        }
    }

    // Measure the pixel dimensions of a string.
    public Vector2 MeasureString(string text)
    {
        float lineW = 0f, maxW = 0f, h = _cellH;
        foreach (char c in text)
        {
            if (c == '\n')
            {
                if (lineW > maxW) maxW = lineW;
                lineW = 0;
                h += _cellH + 1;
                continue;
            }
            int idx = c - FirstChar;
            lineW += idx >= 0 && idx < _advances.Length ? _advances[idx] : _advances[0];
        }
        if (lineW > maxW) maxW = lineW;
        return new Vector2(maxW, h);
    }
}
