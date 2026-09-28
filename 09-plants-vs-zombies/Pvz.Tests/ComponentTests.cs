using Pvz4;
using Pvz4.Components;
using Xunit;

namespace Pvz.Tests;

// A component is one small part with one job, so it can be tested on its own: an entity with
// only the parts under test, no lawn, no textures and no game running. The world is null
// because none of these components use it, and the armour's sprite is null because only
// Draw needs it.
public class ComponentTests
{
    [Fact]
    public void Damage_lowers_health()
    {
        var zombie = new Entity(null);
        Health health = zombie.Add(new Health(10));

        health.Damage(3);

        Assert.Equal(7, health.Current);
        Assert.False(zombie.IsRemoved);
    }

    [Fact]
    public void Armour_takes_the_damage_first_and_passes_on_the_rest()
    {
        var conehead = new Entity(null);
        Armour cone = conehead.Add(new Armour(5, null));
        Health health = conehead.Add(new Health(10));

        health.Damage(8);

        Assert.False(cone.IsIntact);
        Assert.Equal(7, health.Current);
    }

    [Fact]
    public void An_entity_whose_health_runs_out_is_removed()
    {
        var zombie = new Entity(null);
        Health health = zombie.Add(new Health(10));

        health.Damage(10);

        Assert.True(zombie.IsRemoved);
    }

    [Fact]
    public void A_lifetime_removes_its_entity_when_the_time_is_up()
    {
        var sun = new Entity(null).With(new Lifetime(8));

        sun.Update(7.9f);
        Assert.False(sun.IsRemoved);

        sun.Update(0.2f);
        Assert.True(sun.IsRemoved);
    }
}
