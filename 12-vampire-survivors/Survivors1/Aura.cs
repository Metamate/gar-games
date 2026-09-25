using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Survivors1;

// A glowing circle around the player that hurts every enemy inside it, a few times a second.
public sealed class Aura(TextureRegion sprite)
{
    private const float Tick = 0.4f;
    private float _timer;

    public float Radius { get; set; } = 80;
    public float Damage { get; set; } = 1;

    public void Update(float deltaSeconds, Swarm swarm, Vector2 origin)
    {
        _timer -= deltaSeconds;
        if (_timer > 0)
            return;

        _timer = Tick;
        foreach (Enemy enemy in swarm.Within(origin, Radius))
            enemy.Health -= Damage;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 origin)
    {
        float scale = Radius * 2 / sprite.Width;
        sprite.Draw(spriteBatch, origin, Color.White, 0, new Vector2(sprite.Width / 2f, sprite.Height / 2f), scale, SpriteEffects.None, 0);
    }
}
