using Box2D.NET;
using Microsoft.Xna.Framework;
using static Box2D.NET.B2Bodies;
using static Box2D.NET.B2Joints;

namespace Birds2.Physics;

public enum JointType { Weld, Hinge, Rope }

// Adapter: a Box2D joint, holding two bodies together. Its anchors are in pixels, in the world.
public sealed class PhysicsJoint
{
    internal PhysicsJoint(B2JointId id, JointType type, PhysicsBody a, PhysicsBody b)
    {
        Id = id;
        Type = type;
        BodyA = a;
        BodyB = b;
    }

    internal B2JointId Id { get; }
    public JointType Type { get; }
    public PhysicsBody BodyA { get; }
    public PhysicsBody BodyB { get; }

    public Vector2 AnchorA => Units.ToPixels(b2Body_GetWorldPoint(BodyA.Id, b2Joint_GetLocalFrameA(Id).p));
    public Vector2 AnchorB => Units.ToPixels(b2Body_GetWorldPoint(BodyB.Id, b2Joint_GetLocalFrameB(Id).p));
}
