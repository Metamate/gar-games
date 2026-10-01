using Pvz4.Components;
using GARCore.Graphics;
using Microsoft.Xna.Framework;

namespace Pvz4;

// The things that aren't types in the data files: arrows, coins and effects.
public class Recipes(TextureAtlas atlas)
{
    public Entity Arrow(World world, Vector2 position, int row, float damage)
        => new Entity(world) { Position = position, Row = row }
            .With(new SpriteRenderer(atlas.GetRegion("arrow"), centered: true))
            .With(new Projectile(300, damage));

    public Entity Coin(World world, Vector2 position, int amount)
        => new Entity(world) { Position = position }
            .With(new SpriteRenderer(atlas.GetRegion("coin"), centered: true))
            .With(new Collectible(amount));

    public Entity Explosion(World world, Vector2 position)
        => new Entity(world) { Position = position }
            .With(new SpriteRenderer(atlas.GetRegion("explosion"), centered: true))
            .With(new Lifetime(0.4f));

    // Gold from the sky: falls to a random spot on the field.
    public Entity FallingCoin(World world, float x, float targetY)
        => Coin(world, new Vector2(x, 100), 25).With(new Faller(targetY, 60));
}
