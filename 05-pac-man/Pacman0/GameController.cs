using GMDCore;
using Microsoft.Xna.Framework.Input;

namespace Pacman0;

// Maps keys to the game's actions, as in Snake. W/A/S/D and the arrow keys both steer.
public static class GameController
{
    public static bool Up => WasPressed(Keys.W) || WasPressed(Keys.Up);
    public static bool Down => WasPressed(Keys.S) || WasPressed(Keys.Down);
    public static bool Left => WasPressed(Keys.A) || WasPressed(Keys.Left);
    public static bool Right => WasPressed(Keys.D) || WasPressed(Keys.Right);

    private static bool WasPressed(Keys key) => Core.Input.Keyboard.WasKeyJustPressed(key);
}
