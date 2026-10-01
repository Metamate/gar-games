using GARCore;
using Microsoft.Xna.Framework.Input;
using Zelda6.Entities;

namespace Zelda6.Input;

public static class GameController
{
    public static bool Confirm    => Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter);
    public static bool SwingSword => Core.Input.Keyboard.WasKeyJustPressed(Keys.Space);
    public static bool Left       => Core.Input.Keyboard.IsKeyDown(Keys.Left)  || Core.Input.Keyboard.IsKeyDown(Keys.A);
    public static bool Right      => Core.Input.Keyboard.IsKeyDown(Keys.Right) || Core.Input.Keyboard.IsKeyDown(Keys.D);
    public static bool Up         => Core.Input.Keyboard.IsKeyDown(Keys.Up)    || Core.Input.Keyboard.IsKeyDown(Keys.W);
    public static bool Down       => Core.Input.Keyboard.IsKeyDown(Keys.Down)  || Core.Input.Keyboard.IsKeyDown(Keys.S);

    private static readonly Direction[] Directions = [Direction.Left, Direction.Right, Direction.Up, Direction.Down];

    // The direction to walk in, or null when no direction key is held. A key that was just
    // pressed wins, so the newest key decides. Otherwise the current direction goes on while
    // its key is held, and after that any key that is still held.
    public static Direction? WalkDirection(Direction current)
    {
        foreach (Direction direction in Directions)
        {
            if (JustPressed(direction))
                return direction;
        }

        if (Held(current))
            return current;

        foreach (Direction direction in Directions)
        {
            if (Held(direction))
                return direction;
        }

        return null;
    }

    private static bool Held(Direction direction) => direction switch
    {
        Direction.Left  => Left,
        Direction.Right => Right,
        Direction.Up    => Up,
        _               => Down
    };

    private static bool JustPressed(Direction direction) => direction switch
    {
        Direction.Left  => JustPressed(Keys.Left,  Keys.A),
        Direction.Right => JustPressed(Keys.Right, Keys.D),
        Direction.Up    => JustPressed(Keys.Up,    Keys.W),
        _               => JustPressed(Keys.Down,  Keys.S)
    };

    private static bool JustPressed(Keys arrow, Keys letter) =>
        Core.Input.Keyboard.WasKeyJustPressed(arrow) || Core.Input.Keyboard.WasKeyJustPressed(letter);
}
