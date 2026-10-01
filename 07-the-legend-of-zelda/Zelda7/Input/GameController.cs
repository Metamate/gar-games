using GARCore;
using Microsoft.Xna.Framework.Input;
using Zelda7.Entities;

namespace Zelda7.Input;

public static class GameController
{
    public static bool Confirm    => Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter);
    public static bool SwingSword => Core.Input.Keyboard.WasKeyJustPressed(Keys.Space);
    public static bool ToggleDebug => Core.Input.Keyboard.WasKeyJustPressed(Keys.F1);
    public static bool Left       => Core.Input.Keyboard.IsKeyDown(Keys.Left)  || Core.Input.Keyboard.IsKeyDown(Keys.A);
    public static bool Right      => Core.Input.Keyboard.IsKeyDown(Keys.Right) || Core.Input.Keyboard.IsKeyDown(Keys.D);
    public static bool Up         => Core.Input.Keyboard.IsKeyDown(Keys.Up)    || Core.Input.Keyboard.IsKeyDown(Keys.W);
    public static bool Down       => Core.Input.Keyboard.IsKeyDown(Keys.Down)  || Core.Input.Keyboard.IsKeyDown(Keys.S);

    // The keys for each direction.
    private static readonly (Direction Direction, Keys Arrow, Keys Letter)[] WalkKeys =
    [
        (Direction.Left, Keys.Left, Keys.A), (Direction.Right, Keys.Right, Keys.D),
        (Direction.Up,   Keys.Up,   Keys.W), (Direction.Down,  Keys.Down,  Keys.S)
    ];

    // The direction to walk in, or null when no direction key is held. A key that was just
    // pressed wins, so the newest key decides. Otherwise the current direction goes on while
    // its key is held, and after that any key that is still held.
    public static Direction? WalkDirection(Direction current)
    {
        Direction? held = null;
        foreach (var (direction, arrow, letter) in WalkKeys)
        {
            if (Core.Input.Keyboard.WasKeyJustPressed(arrow) || Core.Input.Keyboard.WasKeyJustPressed(letter))
                return direction;

            bool down = Core.Input.Keyboard.IsKeyDown(arrow) || Core.Input.Keyboard.IsKeyDown(letter);
            if (down && (held == null || direction == current))
                held = direction;
        }
        return held;
    }
}
