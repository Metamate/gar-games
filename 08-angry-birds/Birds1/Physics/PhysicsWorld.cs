using System;
using System.Collections.Generic;
using Box2D.NET;
using Microsoft.Xna.Framework;
using static Box2D.NET.B2Bodies;
using static Box2D.NET.B2Geometries;
using static Box2D.NET.B2Joints;
using static Box2D.NET.B2MathFunction;
using static Box2D.NET.B2Shapes;
using static Box2D.NET.B2Types;
using static Box2D.NET.B2Worlds;

namespace Birds1.Physics;

// Facade: one simple class in front of Box2D's many functions. The game asks for a box or a
// circle, and steps the world; everything else about Box2D stays in here.
public sealed class PhysicsWorld : IDisposable
{
    // Box2D wants the same time step every time: a fixed timestep, as in Snake.
    private const float TimeStep = 1 / 60f;
    private const int SubSteps = 4;

    private readonly B2WorldId _world;
    private readonly List<PhysicsBody> _bodies = [];
    private readonly List<PhysicsJoint> _joints = [];
    private float _accumulator;

    public PhysicsWorld(float gravity)
    {
        B2WorldDef worldDef = b2DefaultWorldDef();
        worldDef.gravity = new B2Vec2(0, -Units.ToMeters(gravity));
        _world = b2CreateWorld(in worldDef);
    }

    public IReadOnlyList<PhysicsBody> Bodies => _bodies;

    // Nothing is moving any more: every body has gone to sleep.
    public bool IsSettled => b2World_GetAwakeBodyCount(_world) == 0;

    public PhysicsBody CreateBox(Vector2 center, Vector2 size, float rotation, PhysicsMaterial material, object owner = null, BodyType type = BodyType.Dynamic)
    {
        B2BodyDef bodyDef = b2DefaultBodyDef();
        bodyDef.type = type switch
        {
            BodyType.Static => B2BodyType.b2_staticBody,
            BodyType.Kinematic => B2BodyType.b2_kinematicBody,
            _ => B2BodyType.b2_dynamicBody,
        };
        bodyDef.position = Units.ToMeters(center);
        bodyDef.rotation = b2MakeRot(-rotation);
        B2BodyId id = b2CreateBody(_world, in bodyDef);

        B2Polygon box = b2MakeBox(Units.ToMeters(size.X / 2), Units.ToMeters(size.Y / 2));
        B2ShapeDef shapeDef = ShapeDef(material);
        b2CreatePolygonShape(id, in shapeDef, in box);

        return Add(new PhysicsBody(id, size, 0, type, owner));
    }

    public PhysicsBody CreateCircle(Vector2 center, float radius, PhysicsMaterial material, object owner = null, bool isBullet = false)
    {
        B2BodyDef bodyDef = b2DefaultBodyDef();
        bodyDef.type = B2BodyType.b2_dynamicBody;
        bodyDef.position = Units.ToMeters(center);
        bodyDef.isBullet = isBullet;    // fast: check for collisions between steps too
        B2BodyId id = b2CreateBody(_world, in bodyDef);

        B2Circle circle = new() { center = new B2Vec2(0, 0), radius = Units.ToMeters(radius) };
        B2ShapeDef shapeDef = ShapeDef(material);
        b2CreateCircleShape(id, in shapeDef, in circle);

        return Add(new PhysicsBody(id, Vector2.Zero, radius, BodyType.Dynamic, owner));
    }

    public void Destroy(PhysicsBody body)
    {
        if (_bodies.Remove(body))
        {
            _joints.RemoveAll(j => j.BodyA == body || j.BodyB == body);   // Box2D destroys them with the body
            b2DestroyBody(body.Id);
        }
    }

    // Joints hold two bodies together. Anchors are points in the world, in pixels.

    // Weld: the two bodies act as one, as they are now.
    public PhysicsJoint Weld(PhysicsBody a, PhysicsBody b, Vector2 anchor)
    {
        B2WeldJointDef def = b2DefaultWeldJointDef();
        SetBodies(ref def.@base, a, b, anchor, anchor);
        return AddJoint(new PhysicsJoint(b2CreateWeldJoint(_world, in def), JointType.Weld, a, b));
    }

    // Hinge: b turns around the anchor, like a door or a pendulum.
    public PhysicsJoint Hinge(PhysicsBody a, PhysicsBody b, Vector2 anchor)
    {
        B2RevoluteJointDef def = b2DefaultRevoluteJointDef();
        SetBodies(ref def.@base, a, b, anchor, anchor);
        return AddJoint(new PhysicsJoint(b2CreateRevoluteJoint(_world, in def), JointType.Hinge, a, b));
    }

    // Rope: the anchors can come closer, but never further apart than the length.
    public PhysicsJoint Rope(PhysicsBody a, PhysicsBody b, Vector2 anchorA, Vector2 anchorB, float length)
    {
        B2DistanceJointDef def = b2DefaultDistanceJointDef();
        SetBodies(ref def.@base, a, b, anchorA, anchorB);
        def.length = Units.ToMeters(length);
        def.enableSpring = true;        // a spring with no stiffness: the rope can go slack
        def.hertz = 0;
        def.enableLimit = true;         // but it can't stretch past its length
        def.minLength = 0;
        def.maxLength = Units.ToMeters(length);
        return AddJoint(new PhysicsJoint(b2CreateDistanceJoint(_world, in def), JointType.Rope, a, b));
    }

    public IReadOnlyList<PhysicsJoint> Joints => _joints;

    // Each body gets a frame at its anchor, turned so the joint starts from how the bodies are now.
    // (Only the bodies and frames change: Box2D's defaults for the rest stay.)
    private static void SetBodies(ref B2JointDef def, PhysicsBody a, PhysicsBody b, Vector2 anchorA, Vector2 anchorB)
    {
        def.bodyIdA = a.Id;
        def.bodyIdB = b.Id;
        def.localFrameA = Frame(a.Id, anchorA);
        def.localFrameB = Frame(b.Id, anchorB);
    }

    private static B2Transform Frame(B2BodyId body, Vector2 anchor)
    {
        B2Rot rotation = b2Body_GetRotation(body);
        return new B2Transform { p = b2Body_GetLocalPoint(body, Units.ToMeters(anchor)), q = b2MakeRot(-b2Rot_GetAngle(in rotation)) };
    }

    private PhysicsJoint AddJoint(PhysicsJoint joint)
    {
        _joints.Add(joint);
        return joint;
    }

    public void Update(float deltaSeconds)
    {
        _accumulator += MathHelper.Min(deltaSeconds, 0.25f);
        while (_accumulator >= TimeStep)
        {
            b2World_Step(_world, TimeStep, SubSteps);
            _accumulator -= TimeStep;
        }
    }

    public void Dispose() => b2DestroyWorld(_world);

    private static B2ShapeDef ShapeDef(PhysicsMaterial material)
    {
        B2ShapeDef shapeDef = b2DefaultShapeDef();
        shapeDef.density = material.Density;
        shapeDef.material.friction = material.Friction;
        shapeDef.material.restitution = material.Restitution;
        return shapeDef;
    }

    private PhysicsBody Add(PhysicsBody body)
    {
        b2Body_SetUserData(body.Id, B2UserData.Ref(body));
        _bodies.Add(body);
        return body;
    }
}
