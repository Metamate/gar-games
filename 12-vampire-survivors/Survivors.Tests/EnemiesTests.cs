using Survivors4;
using Microsoft.Xna.Framework;
using Xunit;

namespace Survivors.Tests;

public class EnemiesTests
{
    [Fact]
    public void Removing_an_enemy_moves_the_last_one_into_its_place()
    {
        var enemies = new Enemies(10);
        enemies.Add(0, new Vector2(1, 1));
        enemies.Add(1, new Vector2(2, 2));
        enemies.Add(2, new Vector2(3, 3));

        enemies.RemoveAt(0);

        Assert.Equal(2, enemies.Count);
        Assert.Equal(new Vector2(3, 3), enemies.Position[0]);
        Assert.Equal(2, enemies.Kind[0]);
        Assert.Equal(EnemyKind.All[2].Health, enemies.Health[0]);
        Assert.Equal(EnemyKind.All[2].Radius, enemies.Radius[0]);
        Assert.Equal(new Vector2(2, 2), enemies.Position[1]);
    }

    [Fact]
    public void A_full_store_ignores_new_enemies()
    {
        var enemies = new Enemies(2);
        enemies.Add(0, Vector2.Zero);
        enemies.Add(0, Vector2.Zero);

        enemies.Add(0, Vector2.One);

        Assert.Equal(2, enemies.Count);
        Assert.True(enemies.IsFull);
    }
}
