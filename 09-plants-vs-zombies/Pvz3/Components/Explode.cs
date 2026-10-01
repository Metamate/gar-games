using System.Linq;
using Microsoft.Xna.Framework;

namespace Pvz3.Components;

// After a short fuse, damages every goblin nearby, and is gone.
public class Explode(float fuse, float radius, float damage) : Component
{
    private float _elapsed;

    public override void Update(float deltaSeconds)
    {
        _elapsed += deltaSeconds;
        if (_elapsed < fuse)
            return;

        Vector2 center = Owner.Position + new Vector2(0, -40);
        foreach (Entity goblin in World.GoblinsWithin(center, radius).ToList())
            goblin.Get<Health>()?.Damage(damage);
        World.Add(World.Recipes.Explosion(World, center));
        Owner.Remove();
    }
}
