using System;
using System.Collections.Generic;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario7.States.PlayerStates;
using Mario7.LevelMaker;

namespace Mario7.Entities;

public class MysteryBox(GameLevel level, TextureRegion region, Vector2 position, List<TextureRegion> coins) : IEntity
{
    private List<TextureRegion> coins = coins;
    public GameLevel Level { get; } = level;
    public bool Collidable { get; set; } = true;
    public bool IsSolid => true;
    public bool Active { get; set; } = true;
    public Vector2 Position { get; set; } = position;
    public TextureRegion Region { get; set; } = region;
    public bool WasHit { get; private set; }

    public Rectangle Bounds => new((int)Position.X, (int)Position.Y, Region.Width, Region.Height);

    public void Update(GameTime gameTime)
    {
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Visual feedback when the box is "depleted"
        Color color = WasHit ? Color.Gray : Color.White;
        Region.Draw(spriteBatch, Position, color);
    }

    public bool Collides(IEntity other)
    {
        if (!Active || !Collidable || !other.Collidable) return false;

        // Use a 1-pixel sensor to bridge the gap between "touching" and "actual intersection"
        Rectangle sensor = Bounds;
        sensor.Height += 1;

        bool isSensorTouching = sensor.Intersects(other.Bounds);

        if (isSensorTouching && other is Player player)
        {
            if (player.State is PlayerJumpState)
            {
                // Create a smaller "head" area to prevent hit-from-side triggers
                int horizontalInset = 1;
                Rectangle headArea = player.Bounds;
                headArea.X += horizontalInset;
                headArea.Width -= horizontalInset * 2;

                if (sensor.Intersects(headArea) && player.Bounds.Top >= Bounds.Bottom)
                {
                    if (!WasHit)
                    {
                        OnHit();
                    }
                }
            }
        }

        return isSensorTouching;
    }

    private void OnHit()
    {
        WasHit = true;

        if (coins != null && coins.Count > 0)
        {
            var coinRegion = coins[Random.Shared.Next(coins.Count)];
            // Spawn coin above the box with an upward pop and slight horizontal variance
            Vector2 coinPos = new(Position.X, Position.Y - coinRegion.Height);
            float randomX = (float)(Random.Shared.NextDouble() * 100 - 50); // -50 to 50 spread
            Level.AddEntity(new Coin(coinRegion, coinPos, new Vector2(randomX, -200f)));
        }
    }
}
