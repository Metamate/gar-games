using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GARCore.Input;

public class MouseInfo
{
    public MouseState PreviousState { get; private set; }
    public MouseState CurrentState { get; private set; }

    public MouseInfo()
    {
        PreviousState = new MouseState();
        CurrentState = Mouse.GetState();
    }

    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState = Mouse.GetState();
    }

    // The position in window coordinates. The game's virtual resolution is a different space.
    public int X => CurrentState.X;
    public int Y => CurrentState.Y;
    public Point Position => CurrentState.Position;

    public bool IsLeftButtonDown
        => CurrentState.LeftButton == ButtonState.Pressed;

    public bool WasLeftButtonJustPressed
        => CurrentState.LeftButton == ButtonState.Pressed && PreviousState.LeftButton == ButtonState.Released;
}