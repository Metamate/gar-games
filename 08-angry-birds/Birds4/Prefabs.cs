using System.Collections.Generic;
using Birds4.Entities;
using Birds4.Physics;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;

namespace Birds4;

// The prototypes: one configured entity per kind of thing in a level, by name. A level asks
// for "wood-plank", and gets a copy of this plank, placed in the world.
public class Prefabs
{
    private readonly Dictionary<string, Entity> _prototypes = [];

    public Prefabs(TextureAtlas atlas)
    {
        var plank = new Vector2(180, 20);
        var post = new Vector2(20, 100);
        var box = new Vector2(50, 50);

        foreach (var (name, material, health) in new[] { ("wood", Materials.Wood, 8f), ("stone", Materials.Stone, 16f), ("glass", Materials.Glass, 4f) })
        {
            Add($"{name}-plank", new Block(material, plank, health, 500, atlas.GetRegion($"{name}-plank")));
            Add($"{name}-post", new Block(material, post, health, 500, atlas.GetRegion($"{name}-post")));
            Add($"{name}-box", new Block(material, box, health, 500, atlas.GetRegion($"{name}-box")));
        }

        Add("pig", new Pig(22, 3, 5000, atlas.GetRegion("pig")));
        Add("big-pig", new Pig(30, 6, 5000, atlas.GetRegion("big-pig")));
        Add("bird", new Bird(atlas.GetRegion("bird")));
    }

    public void Add(string name, Entity prototype) => _prototypes[name] = prototype;

    public Entity Spawn(string name, PhysicsWorld world, Vector2 position, float rotation = 0)
    {
        Entity entity = _prototypes[name].Clone();
        entity.Spawn(world, position, rotation);
        return entity;
    }
}
