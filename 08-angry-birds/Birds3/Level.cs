using System.Collections.Generic;
using System.Globalization;
using Birds3.Entities;
using Birds3.Physics;
using Microsoft.Xna.Framework;

namespace Birds3;

// A level file: how many birds, and which prefab goes where. Levels are data, as in Sokoban.
public class Level
{
    public record Placement(string Prefab, Vector2 Position, float Rotation);

    public int Birds { get; private set; } = 3;
    public List<Placement> Placements { get; } = [];

    public static Level Parse(string text)
    {
        var level = new Level();
        foreach (string line in text.Replace("\r", "").Split('\n'))
        {
            string[] parts = line.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0 || parts[0].StartsWith('#'))
                continue;

            if (parts[0] == "birds")
            {
                level.Birds = int.Parse(parts[1]);
                continue;
            }

            var position = new Vector2(Number(parts[1]), Number(parts[2]));
            float rotation = MathHelper.ToRadians(Number(parts[3]));
            level.Placements.Add(new Placement(parts[0], position, rotation));
        }
        return level;
    }

    public List<Entity> Spawn(Prefabs prefabs, PhysicsWorld world)
    {
        var entities = new List<Entity>();
        foreach (Placement placement in Placements)
            entities.Add(prefabs.Spawn(placement.Prefab, world, placement.Position, placement.Rotation));
        return entities;
    }

    private static float Number(string text) => float.Parse(text, CultureInfo.InvariantCulture);
}
