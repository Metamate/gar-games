using Pvz4;
using Pvz4.Components;
using Xunit;

namespace Pvz.Tests;

// A component is one small part with one job, so it can be tested on its own: an entity with
// only the parts under test, no field, no textures and no game running. The world is null
// because none of these components use it, and the armour's sprite is null because only
// Draw needs it.
public class ComponentTests
{
    [Fact]
    public void Damage_lowers_health()
    {
        var goblin = new Entity(null);
        Health health = goblin.Add(new Health(10));

        health.Damage(3);

        Assert.Equal(7, health.Current);
        Assert.False(goblin.IsRemoved);
    }

    [Fact]
    public void Armour_takes_the_damage_first_and_passes_on_the_rest()
    {
        var shieldbearer = new Entity(null);
        Armour shield = shieldbearer.Add(new Armour(5, null));
        Health health = shieldbearer.Add(new Health(10));

        health.Damage(8);

        Assert.False(shield.IsIntact);
        Assert.Equal(7, health.Current);
    }

    [Fact]
    public void An_entity_whose_health_runs_out_is_removed()
    {
        var goblin = new Entity(null);
        Health health = goblin.Add(new Health(10));

        health.Damage(10);

        Assert.True(goblin.IsRemoved);
    }

    [Fact]
    public void A_lifetime_removes_its_entity_when_the_time_is_up()
    {
        var coin = new Entity(null).With(new Lifetime(8));

        coin.Update(7.9f);
        Assert.False(coin.IsRemoved);

        coin.Update(0.2f);
        Assert.True(coin.IsRemoved);
    }
}
