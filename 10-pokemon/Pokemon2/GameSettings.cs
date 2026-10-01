using Microsoft.Xna.Framework;

namespace Pokemon2;

public static class GameSettings
{
    public const int WindowWidth = 1280;
    public const int WindowHeight = 720;
    public const int VirtualWidth = 320;
    public const int VirtualHeight = 180;
    public const int TileSize = 16;

    // The game's four shades, darkest to lightest, like a handheld's screen. Every colour on
    // screen is one of them: the art's, the panels', the text's.
    public static readonly Color Ink    = new(68, 65, 93);
    public static readonly Color Shadow = new(102, 99, 137);
    public static readonly Color Mid    = new(142, 139, 183);
    public static readonly Color Paper  = new(181, 176, 221);

    // Level dimensions in tiles (what fits the 320×180 screen with 16px tiles)
    public const int MapCols = 20;
    public const int MapRows = 11;

    public const int PlayerStartMapX = 8;
    public const int PlayerStartMapY = 6;

    public static readonly int[] TileGrass = { 45, 46 };
    public const int TileTallGrass = 41;
    public const int TileTree = 0;
    public const int TileFence = 1;
    public const int TileSign = 2;
    public const int TileFlowers = 3;
    public const int TilePath = 4;
    // The top-left tile of each house: 3 x 3 tiles in the tilesheet.
    public const int TileFlatHouse = 8;
    public const int TilePeakedHouse = 11;

    // Time (seconds) to tween one tile-step walk
    public const float WalkTweenDuration = 0.5f;

    // 1-in-N chance of a random encounter when stepping into tall grass
    public const int EncounterChance = 10;

    // Pokemon battle
    public const int PlayerStartLevel = 5;
    public const int OpponentLevelMin = 2;
    public const int OpponentLevelMax = 6; // exclusive upper bound

    // Battle intro slide-in duration (seconds)
    public const float BattleSlideInDuration = 1f;

    // Fade transition duration (seconds)
    public const float FadeDuration = 1f;

    // GUI
    public const int TextboxPadding = 4;  // inner padding from panel edge
    public const int TextboxLinesPerPage = 3;

    // Selection menu cursor sprite size
    public const int CursorWidth = 8;
}
