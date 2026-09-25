using System;
using System.Collections.Generic;
using Box2D.NET;
using Microsoft.Xna.Framework;
using static Box2D.NET.B2Bodies;
using static Box2D.NET.B2Geometries;
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

    public PhysicsBody CreateBox(Vector2 center, Vector2 size, float rotation, PhysicsMaterial material, object owner = null, bool isStatic = false)
    {
        B2BodyDef bodyDef = b2DefaultBodyDef();
        bodyDef.type = isStatic ? B2BodyType.b2_staticBody : B2BodyType.b2_dynamicBody;
        bodyDef.position = Units.ToMeters(center);
        bodyDef.rotation = b2MakeRot(-rotation);
        B2BodyId id = b2CreateBody(_world, in bodyDef);

        B2Polygon box = b2MakeBox(Units.ToMeters(size.X / 2), Units.ToMeters(size.Y / 2));
        B2ShapeDef shapeDef = ShapeDef(material);
        b2CreatePolygonShape(id, in shapeDef, in box);

        return Add(new PhysicsBody(id, size, 0, isStatic, owner));
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

        return Add(new PhysicsBody(id, Vector2.Zero, radius, false, owner));
    }

    public void Destroy(PhysicsBody body)
    {
        if (_bodies.Remove(body))
            b2DestroyBody(body.Id);
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
