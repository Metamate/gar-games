using System.Collections.Generic;
using System.Text.Json;
using Pvz3.Components;

namespace Pvz3;

// Type Object: one PlantType per kind of plant, loaded from plants.json. Every sunflower on the
// lawn shares the one Sunflower type: its cost, its recharge time, and how to build one.
public class PlantType
{
    public string Name { get; init; }
    public string Sprite { get; init; }
    public int Cost { get; init; }
    public float Recharge { get; init; }
    public float Health { get; init; }

    // A plant gets a component for each of these that the data has.
    public ShooterData Shooter { get; init; }
    public SunProducerData SunProducer { get; init; }
    public ExplodeData Explode { get; init; }

    // The type makes its own instances.
    public Entity Create(World world)
    {
        var plant = new Entity(world)
            .With(new SpriteRenderer(world.Atlas.GetRegion(Sprite)))
            .With(new Health(Health));

        if (Shooter != null)
            plant.Add(new Shooter(Shooter.Interval, Shooter.Shots, Shooter.Damage));
        if (SunProducer != null)
            plant.Add(new SunProducer(SunProducer.Interval, SunProducer.Amount));
        if (Explode != null)
            plant.Add(new Explode(Explode.Fuse, Explode.Radius, Explode.Damage));
        return plant;
    }
}

public record ShooterData(float Interval, int Shots, float Damage);
public record SunProducerData(float Interval, int Amount);
public record ExplodeData(float Fuse, float Radius, float Damage);

// One ZombieType per kind of zombie, loaded from zombies.json.
public class ZombieType
{
    public string Name { get; init; }
    public string Sprite { get; init; }
    public float Health { get; init; }
    public float Speed { get; init; }
    public float Bite { get; init; }
    public ArmourData Armour { get; init; }

    public Entity Create(World world)
    {
        var zombie = new Entity(world)
            .With(new SpriteRenderer(world.Atlas.GetRegion(Sprite)))
            .With(new Health(Health))
            .With(new Walker(Speed))
            .With(new Eater(Bite));

        if (Armour != null)
            zombie.Add(new Armour(Armour.Health, world.Atlas.GetRegion(Armour.Sprite)));
        return zombie;
    }
}

public record ArmourData(string Sprite, float Health);

public static class GameData
{
    private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

    public static List<T> Load<T>(string json) => JsonSerializer.Deserialize<List<T>>(json, Options);
    public static T LoadOne<T>(string json) => JsonSerializer.Deserialize<T>(json, Options);
}
