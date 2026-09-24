using GMDCore;
using Microsoft.Xna.Framework.Input;

namespace Sokoban4;

// Maps keys to the game's actions, as in Snake. W/A/S/D and the arrow keys both move.
public static class GameController
{
    public static bool Up => WasPressed(Keys.W) || WasPressed(Keys.Up);
    public static bool Down => WasPressed(Keys.S) || WasPressed(Keys.Down);
    public static bool Left => WasPressed(Keys.A) || WasPressed(Keys.Left);
    public static bool Right => WasPressed(Keys.D) || WasPressed(Keys.Right);
    public static bool Undo => WasPressed(Keys.Z) || WasPressed(Keys.Back);
    public static bool Redo => WasPressed(Keys.Y);
    public static bool Restart => WasPressed(Keys.R);
    public static bool Continue => WasPressed(Keys.Enter) || WasPressed(Keys.Space);

    private static bool WasPressed(Keys key) => Core.Input.Keyboard.WasKeyJustPressed(key);
}
