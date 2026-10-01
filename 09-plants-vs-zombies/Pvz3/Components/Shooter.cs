using Microsoft.Xna.Framework;

namespace Pvz3.Components;

// Shoots arrows when there's a goblin ahead in the row: a burst of one or more, then a pause.
public class Shooter(float interval, int shots, float damage) : Component
{
    private const float BurstDelay = 0.2f;
    private float _cooldown;
    private float _burstTimer;
    private int _burstLeft;

    public override void Update(float deltaSeconds)
    {
        _cooldown -= deltaSeconds;
        if (_burstLeft > 0)
        {
            _burstTimer -= deltaSeconds;
            if (_burstTimer <= 0)
                Fire();
        }
        else if (_cooldown <= 0 && World.FirstGoblinAhead(Owner.Row, Owner.Position.X) != null)
        {
            _cooldown = interval;
            _burstLeft = shots;
            Fire();
        }
    }

    private void Fire()
    {
        World.Add(World.Recipes.Arrow(World, Owner.Position + new Vector2(30, -40), Owner.Row, damage));
        _burstLeft--;
        _burstTimer = BurstDelay;
    }
}
