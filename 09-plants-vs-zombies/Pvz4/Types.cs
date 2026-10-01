using System.Collections.Generic;
using System.Text.Json;
using Pvz4.Components;

namespace Pvz4;

// Type Object: one DefenderType per kind of defender, loaded from defenders.json. Every chest on the
// field shares the one Chest type: its cost, its recharge time, and how to build one.
public class DefenderType
{
    public string Name { get; init; }
    public string Sprite { get; init; }
    public int Cost { get; init; }
    public float Recharge { get; init; }
    public float Health { get; init; }

    // A defender gets a component for each of these that the data has.
    public ShooterData Shooter { get; init; }
    public GoldProducerData GoldProducer { get; init; }
    public ExplodeData Explode { get; init; }

    // The type makes its own instances.
    public Entity Create(World world)
    {
        var defender = new Entity(world)
            .With(new SpriteRenderer(world.Atlas.GetRegion(Sprite)))
            .With(new Health(Health));

        if (Shooter != null)
            defender.Add(new Shooter(Shooter.Interval, Shooter.Shots, Shooter.Damage));
        if (GoldProducer != null)
            defender.Add(new GoldProducer(GoldProducer.Interval, GoldProducer.Amount));
        if (Explode != null)
            defender.Add(new Explode(Explode.Fuse, Explode.Radius, Explode.Damage));
        return defender;
    }
}

public record ShooterData(float Interval, int Shots, float Damage);
public record GoldProducerData(float Interval, int Amount);
public record ExplodeData(float Fuse, float Radius, float Damage);

// One GoblinType per kind of goblin, loaded from goblins.json.
public class GoblinType
{
    public string Name { get; init; }
    public string Sprite { get; init; }
    public float Health { get; init; }
    public float Speed { get; init; }
    public float Attack { get; init; }
    public ArmourData Armour { get; init; }

    public Entity Create(World world)
    {
        var goblin = new Entity(world)
            .With(new SpriteRenderer(world.Atlas.GetRegion(Sprite)))
            .With(new Health(Health))
            .With(new Walker(Speed))
            .With(new Attacker(Attack));

        if (Armour != null)
            goblin.Add(new Armour(Armour.Health, world.Atlas.GetRegion(Armour.Sprite)));
        return goblin;
    }
}

public record ArmourData(string Sprite, float Health);

public static class GameData
{
    private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

    public static List<T> Load<T>(string json) => JsonSerializer.Deserialize<List<T>>(json, Options);
    public static T LoadOne<T>(string json) => JsonSerializer.Deserialize<T>(json, Options);
}
