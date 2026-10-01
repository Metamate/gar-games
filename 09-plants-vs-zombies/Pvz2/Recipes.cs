using Pvz2.Components;
using GARCore.Graphics;
using Microsoft.Xna.Framework;

namespace Pvz2;

// Recipes: each kind of thing in the game is an entity with a list of components. There is no
// class per defender or goblin. A Wizard is an Archer with a different number; a Shieldbearer
// is a Goblin with an Armour.
public class Recipes(TextureAtlas atlas)
{
    public Entity Chest(World world) => Defender(world, "chest", 6).With(new GoldProducer(9, 25));
    public Entity Archer(World world) => Defender(world, "archer", 6).With(new Shooter(1.4f, 1, 1));
    public Entity Wizard(World world) => Defender(world, "wizard", 6).With(new Shooter(1.4f, 2, 1));
    public Entity Knight(World world) => Defender(world, "knight", 40);

    public Entity Goblin(World world)
        => new Entity(world)
            .With(new SpriteRenderer(atlas.GetRegion("goblin")))
            .With(new Health(10))
            .With(new Walker(28))
            .With(new Attacker(1));

    public Entity Shieldbearer(World world) => Goblin(world).With(new Armour(18, atlas.GetRegion("wooden-shield")));

    public Entity Arrow(World world, Vector2 position, int row, float damage)
        => new Entity(world) { Position = position, Row = row }
            .With(new SpriteRenderer(atlas.GetRegion("arrow"), centered: true))
            .With(new Projectile(300, damage));

    public Entity Coin(World world, Vector2 position, int amount)
        => new Entity(world) { Position = position }
            .With(new SpriteRenderer(atlas.GetRegion("coin"), centered: true))
            .With(new Collectible(amount));

    private Entity Defender(World world, string sprite, float health)
        => new Entity(world)
            .With(new SpriteRenderer(atlas.GetRegion(sprite)))
            .With(new Health(health));
}
