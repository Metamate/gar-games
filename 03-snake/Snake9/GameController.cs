using GMDCore;
using Microsoft.Xna.Framework.Input;

namespace Snake9;

// Maps keys to the game's actions. The rest of the game asks whether the player wants to go
// up, never which key was pressed, so the controls can change in one place. Here, W/A/S/D and
// the arrow keys both work.
public static class GameController
{
    public static bool Up => WasPressed(Keys.W) || WasPressed(Keys.Up);
    public static bool Down => WasPressed(Keys.S) || WasPressed(Keys.Down);
    public static bool Left => WasPressed(Keys.A) || WasPressed(Keys.Left);
    public static bool Right => WasPressed(Keys.D) || WasPressed(Keys.Right);

    private static bool WasPressed(Keys key) => Core.Input.Keyboard.WasKeyJustPressed(key);
}
