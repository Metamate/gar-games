using Box2D.NET;
using Microsoft.Xna.Framework;
using static Box2D.NET.B2Bodies;
using static Box2D.NET.B2MathFunction;

namespace Birds1.Physics;

// Adapter: a Box2D body, with the interface the game wants. Positions and velocities are in
// pixels, rotations are clockwise, and no Box2D type leaks out.
public sealed class PhysicsBody
{
    internal PhysicsBody(B2BodyId id, Vector2 size, float radius, bool isStatic, object owner)
    {
        Id = id;
        Size = size;
        Radius = radius;
        IsStatic = isStatic;
        Owner = owner;
    }

    internal B2BodyId Id { get; }

    // The game object this body belongs to, if any.
    public object Owner { get; }

    public Vector2 Size { get; }    // for boxes, in pixels
    public float Radius { get; }    // for circles, in pixels (0 for boxes)
    public bool IsCircle => Radius > 0;
    public bool IsStatic { get; }

    public Vector2 Position => Units.ToPixels(b2Body_GetPosition(Id));

    // Box2D turns counter-clockwise, SpriteBatch clockwise.
    public float Rotation
    {
        get
        {
            B2Rot rotation = b2Body_GetRotation(Id);
            return -b2Rot_GetAngle(in rotation);
        }
    }

    public Vector2 Velocity
    {
        get => Units.ToPixels(b2Body_GetLinearVelocity(Id));
        set => b2Body_SetLinearVelocity(Id, Units.ToMeters(value));
    }

    public bool IsAwake => b2Body_IsAwake(Id);
}
