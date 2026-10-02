namespace Zelda1;

public static class GameSettings
{
    public const int WindowWidth   = 1280;
    public const int WindowHeight  = 720;

    public const int VirtualWidth  = 320;
    public const int VirtualHeight = 180;

    public const int TileSize = 16;

    // Map layout: total tiles that fit on screen minus 1 border tile on each side
    public const int MapWidth  = VirtualWidth  / TileSize - 2;
    public const int MapHeight = VirtualHeight / TileSize - 2;

    public const int MapRenderOffsetX = (VirtualWidth  - MapWidth  * TileSize) / 2;
    public const int MapRenderOffsetY = (VirtualHeight - MapHeight * TileSize) / 2;

    // Player
    public const int   PlayerWidth         = 16;
    public const int   PlayerHeight        = 12;
    public const int   PlayerWalkSpeed     = 60;
    public const int   PlayerStartHealth   = 6;     // three hearts × 2 per heart
    public const float PlayerSpriteOffsetY = 12f;   // 8 px of room in the frame for the sword, and the head 4 px above the collision box (perspective)
    public const float PlayerSwordOffsetX  = 8f;    // sword sprite (32px) centred over collision box (16px)

    // Tile IDs (0-based)
    public const int TileTopLeftCorner     =  3;
    public const int TileTopRightCorner    =  4;
    public const int TileBottomLeftCorner  = 22;
    public const int TileBottomRightCorner = 23;

    public static readonly int[] TileFloors =
    [
        6, 7, 8, 9, 10, 11, 12,
        25, 26, 27, 28, 29, 30, 31,
        44, 45, 46, 47, 48, 49, 50,
        63, 64, 65, 66, 67, 68, 69,
        87, 88, 106, 107
    ];

    public static readonly int[] TileTopWalls    = [57, 58, 59];
    public static readonly int[] TileBottomWalls = [78, 79, 80];
    public static readonly int[] TileLeftWalls   = [76, 95, 114];
    public static readonly int[] TileRightWalls  = [77, 96, 115];
}
