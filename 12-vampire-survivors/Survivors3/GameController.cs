using GMDCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Survivors3;

// Maps keys to the game's actions. W/A/S/D and the arrow keys both move.
public static class GameController
{
    public static Vector2 Move
    {
        get
        {
            Vector2 move = Vector2.Zero;
            if (IsDown(Keys.W) || IsDown(Keys.Up)) move.Y -= 1;
            if (IsDown(Keys.S) || IsDown(Keys.Down)) move.Y += 1;
            if (IsDown(Keys.A) || IsDown(Keys.Left)) move.X -= 1;
            if (IsDown(Keys.D) || IsDown(Keys.Right)) move.X += 1;
            return move == Vector2.Zero ? move : Vector2.Normalize(move);
        }
    }

    public static bool Restart => WasPressed(Keys.R);
    public static bool Stress => WasPressed(Keys.Space);
    public static bool ToggleProfiler => WasPressed(Keys.F3);
    private static bool IsDown(Keys key) => Core.Input.Keyboard.IsKeyDown(key);
    private static bool WasPressed(Keys key) => Core.Input.Keyboard.WasKeyJustPressed(key);
}
