using Pvz3.Components;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;

namespace Pvz3;

// The things that aren't types in the data files: peas, suns and effects.
public class Recipes(TextureAtlas atlas)
{
    public Entity Pea(World world, Vector2 position, int row, float damage)
        => new Entity(world) { Position = position, Row = row }
            .With(new SpriteRenderer(atlas.GetRegion("pea"), centered: true))
            .With(new Projectile(300, damage));

    public Entity Sun(World world, Vector2 position, int amount)
        => new Entity(world) { Position = position }
            .With(new SpriteRenderer(atlas.GetRegion("sun"), centered: true))
            .With(new Collectible(amount));

    public Entity Explosion(World world, Vector2 position)
        => new Entity(world) { Position = position }
            .With(new SpriteRenderer(atlas.GetRegion("explosion"), centered: true))
            .With(new Lifetime(0.4f));
}
