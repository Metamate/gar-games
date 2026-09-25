using Pvz2.Components;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;

namespace Pvz2;

// Recipes: each kind of thing in the game is an entity with a list of components. There is no
// class per plant or zombie. A Repeater is a Peashooter with a different number; a Conehead
// is a Zombie with an Armour.
public class Recipes(TextureAtlas atlas)
{
    public Entity Sunflower(World world) => Plant(world, "sunflower", 6).With(new SunProducer(12, 25));
    public Entity Peashooter(World world) => Plant(world, "peashooter", 6).With(new Shooter(1.4f, 1, 1));
    public Entity Repeater(World world) => Plant(world, "repeater", 6).With(new Shooter(1.4f, 2, 1));
    public Entity WallNut(World world) => Plant(world, "wall-nut", 40);

    public Entity Zombie(World world)
        => new Entity(world)
            .With(new SpriteRenderer(atlas.GetRegion("zombie")))
            .With(new Health(10))
            .With(new Walker(16))
            .With(new Eater(1));

    public Entity Conehead(World world) => Zombie(world).With(new Armour(18, atlas.GetRegion("cone")));

    public Entity Pea(World world, Vector2 position, int row, float damage)
        => new Entity(world) { Position = position, Row = row }
            .With(new SpriteRenderer(atlas.GetRegion("pea"), centered: true))
            .With(new Projectile(300, damage));

    public Entity Sun(World world, Vector2 position, int amount)
        => new Entity(world) { Position = position }
            .With(new SpriteRenderer(atlas.GetRegion("sun"), centered: true))
            .With(new Collectible(amount));

    private Entity Plant(World world, string sprite, float health)
        => new Entity(world)
            .With(new SpriteRenderer(atlas.GetRegion(sprite)))
            .With(new Health(health));
}
