using System;
using System.Collections.Generic;
using Birds4;
using Birds4.Physics;
using Microsoft.Xna.Framework;

namespace PhysicsSamples;

// One physics idea per scene. Each scene builds its bodies through the game's facade
// (PhysicsWorld), and may move things each frame.
public abstract class Scene
{
    public abstract string Title { get; }
    public abstract string[] Notes { get; }
    public virtual List<(Vector2 At, string Text)> Labels { get; } = [];

    public abstract void Build(PhysicsWorld world);
    public virtual void Update(PhysicsWorld world, float deltaSeconds) { }

    protected static readonly Random Random = new();

    // Every scene has the same ground.
    protected static void Ground(PhysicsWorld world)
        => world.CreateBox(new Vector2(640, 690), new Vector2(1400, 60), 0, Materials.Ground, type: BodyType.Static);
}

// Static, kinematic and dynamic bodies.
public sealed class BodyTypesScene : Scene
{
    private PhysicsBody _platform;
    private float _dropTimer;

    public override string Title => "1  Body types";
    public override string[] Notes =>
    [
        "Static (orange): never moves. The ground, the ledge.",
        "Kinematic (magenta): moves at the velocity you give it, and pushes, but is never pushed.",
        "Dynamic (green, blue when asleep): moved by gravity and collisions.",
    ];

    public override void Build(PhysicsWorld world)
    {
        Ground(world);
        world.CreateBox(new Vector2(300, 470), new Vector2(300, 20), 0, Materials.Ground, type: BodyType.Static);
        _platform = world.CreateBox(new Vector2(800, 420), new Vector2(220, 20), 0, Materials.Ground, type: BodyType.Kinematic);
        _platform.Velocity = new Vector2(160, 0);
        _dropTimer = 0;
    }

    public override void Update(PhysicsWorld world, float deltaSeconds)
    {
        // A kinematic body only moves as we tell it: turn it round at each end.
        if (_platform.Position.X > 1020) _platform.Velocity = new Vector2(-160, 0);
        if (_platform.Position.X < 620) _platform.Velocity = new Vector2(160, 0);

        _dropTimer -= deltaSeconds;
        if (_dropTimer <= 0 && world.Bodies.Count < 60)
        {
            world.CreateBox(new Vector2(Random.Next(200, 1000), 150), new Vector2(40, 40), 0, Materials.Wood);
            _dropTimer = 0.8f;
        }
    }
}

// Restitution: how much of its speed a body keeps when it bounces.
public sealed class BounceScene : Scene
{
    private static readonly float[] Bounces = [0f, 0.3f, 0.6f, 0.8f, 0.95f];

    public override string Title => "2  Bounce";
    public override string[] Notes =>
    [
        "The same ball five times, dropped from the same height. Only the restitution differs (the",
        "numbers): how much of its speed a body keeps when it bounces. 0 keeps none, 1 all of it.",
    ];

    public override void Build(PhysicsWorld world)
    {
        Ground(world);
        Labels.Clear();
        for (int i = 0; i < Bounces.Length; i++)
        {
            float x = 240 + i * 200;
            world.CreateCircle(new Vector2(x, 220), 25, new PhysicsMaterial(1, 0.5f, Bounces[i]));
            Labels.Add((new Vector2(x - 12, 170), $"{Bounces[i]:0.##}"));
        }
    }
}

// Friction: the same box on the same slope, gripping more or less.
public sealed class FrictionScene : Scene
{
    private const float Slope = 0.45f;                 // radians, about 26 degrees
    private static readonly float[] Frictions = [0f, 0.15f, 0.6f];

    public override string Title => "3  Friction";
    public override string[] Notes =>
    [
        "Three slopes at the same angle, a box on each. Only the boxes' friction differs.",
    ];

    public override void Build(PhysicsWorld world)
    {
        Ground(world);
        Labels.Clear();
        var along = new Vector2(MathF.Cos(Slope), MathF.Sin(Slope));      // down the slope
        var up = new Vector2(MathF.Sin(Slope), -MathF.Cos(Slope));        // away from its surface
        for (int i = 0; i < Frictions.Length; i++)
        {
            var center = new Vector2(640, 240 + i * 140);
            world.CreateBox(center, new Vector2(560, 16), Slope, new PhysicsMaterial(0, 1, 0), type: BodyType.Static);
            Vector2 start = center - along * 220 + up * 29;
            world.CreateBox(start, new Vector2(40, 40), Slope, new PhysicsMaterial(1, Frictions[i], 0));
            Labels.Add((new Vector2(150, center.Y - 130), $"friction {Frictions[i]:0.##}"));
        }
    }
}

// Density: the same size, a different mass.
public sealed class DensityScene : Scene
{
    public override string Title => "4  Density";
    public override string[] Notes =>
    [
        "The same ball, at the same speed, into the same tower. Only the density differs,",
        "so the heavy ball has fifty times the mass.",
    ];

    public override void Build(PhysicsWorld world)
    {
        Ground(world);
        Labels.Clear();
        world.CreateBox(new Vector2(640, 380), new Vector2(1100, 20), 0, Materials.Ground, type: BodyType.Static);
        Tower(world, 370);
        Tower(world, 660);
        world.CreateCircle(new Vector2(150, 345), 25, new PhysicsMaterial(0.2f, 0.6f, 0.2f)).Velocity = new Vector2(700, 0);
        world.CreateCircle(new Vector2(150, 635), 25, new PhysicsMaterial(10f, 0.6f, 0.2f)).Velocity = new Vector2(700, 0);
        Labels.Add((new Vector2(100, 290), "light: density 0.2"));
        Labels.Add((new Vector2(100, 580), "heavy: density 10"));
    }

    private static void Tower(PhysicsWorld world, float top)
    {
        for (int i = 0; i < 4; i++)
            world.CreateBox(new Vector2(950, top - 25 - i * 50), new Vector2(50, 50), 0, Materials.Wood);
    }
}

// Sleeping: bodies at rest stop being simulated. Crates, because a pile of balls keeps rolling.
public sealed class SleepScene : Scene
{
    private int _dropped;
    private float _dropTimer;

    public override string Title => "5  Sleep";
    public override string[] Notes =>
    [
        "A pit of crates. When a body has been still for a moment it goes to sleep (blue):",
        "Box2D stops simulating it until something touches it. Drop a box in to wake them.",
    ];

    public override void Build(PhysicsWorld world)
    {
        Ground(world);
        world.CreateBox(new Vector2(340, 470), new Vector2(20, 420), 0, Materials.Ground, type: BodyType.Static);
        world.CreateBox(new Vector2(940, 470), new Vector2(20, 420), 0, Materials.Ground, type: BodyType.Static);
        _dropped = 0;
        _dropTimer = 0;
    }

    public override void Update(PhysicsWorld world, float deltaSeconds)
    {
        _dropTimer -= deltaSeconds;
        if (_dropTimer <= 0 && _dropped < 70)
        {
            float size = Random.Next(26, 44);
            world.CreateBox(new Vector2(Random.Next(380, 900), 180), new Vector2(size, size), Random.Next(0, 90) * 0.0175f, Materials.Wood);
            _dropped++;
            _dropTimer = 0.06f;
        }
    }
}

// Joints: bodies held together.
public sealed class JointsScene : Scene
{
    public override string Title => "6  Joints";
    public override string[] Notes =>
    [
        "A joint holds two bodies together (yellow: its anchors). A hinge lets them turn around a point,",
        "a rope keeps them within a length but can go slack, and a weld makes two bodies act as one.",
    ];

    public override void Build(PhysicsWorld world)
    {
        Ground(world);
        Labels.Clear();

        // A pendulum: a ball on a hinge, started out to the side.
        PhysicsBody pivot = Pin(world, new Vector2(200, 180));
        PhysicsBody bob = world.CreateCircle(new Vector2(360, 180), 25, Materials.Stone);
        PhysicsBody arm = world.CreateBox(new Vector2(280, 180), new Vector2(160, 6), 0, Materials.Wood);
        world.Hinge(pivot, arm, new Vector2(200, 180));
        world.Weld(arm, bob, new Vector2(360, 180));
        Labels.Add((new Vector2(150, 140), "hinge"));

        // A tether ball: a rope 220 long, slack at the start.
        PhysicsBody hook = Pin(world, new Vector2(520, 180));
        PhysicsBody ball = world.CreateCircle(new Vector2(620, 260), 22, Materials.Wood);
        world.Rope(hook, ball, new Vector2(520, 180), ball.Position, 220);
        Labels.Add((new Vector2(480, 140), "rope"));

        // A chain: eight links, each hinged to the one before.
        PhysicsBody previous = Pin(world, new Vector2(800, 180));
        for (int i = 0; i < 8; i++)
        {
            float x = 800 + 20 + i * 40;
            PhysicsBody link = world.CreateBox(new Vector2(x, 180), new Vector2(40, 8), 0, Materials.Wood);
            world.Hinge(previous, link, new Vector2(x - 20, 180));
            previous = link;
        }
        Labels.Add((new Vector2(760, 140), "hinged chain"));

        // A hammer: a handle and a head, welded into one body, dropped at an angle.
        PhysicsBody handle = world.CreateBox(new Vector2(1060, 330), new Vector2(14, 110), 0.5f, Materials.Wood);
        Vector2 top = handle.Position + new Vector2(System.MathF.Sin(0.5f), -System.MathF.Cos(0.5f)) * 60;
        PhysicsBody head = world.CreateBox(top, new Vector2(60, 26), 0.5f, Materials.Stone);
        world.Weld(handle, head, top);
        Labels.Add((new Vector2(1000, 230), "weld"));
    }

    // A small static body to hang things from.
    private static PhysicsBody Pin(PhysicsWorld world, Vector2 at)
        => world.CreateBox(at, new Vector2(10, 10), 0, Materials.Ground, type: BodyType.Static);
}
