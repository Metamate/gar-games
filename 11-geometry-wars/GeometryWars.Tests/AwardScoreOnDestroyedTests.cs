using GeometryWars6.Components.Combat;
using GeometryWars6.Components.Lifecycle;
using GMDCore.ECS;
using Xunit;

namespace GeometryWars.Tests;

// Tests one component on a bare entity: no world, no graphics, no real score tracker.
public class AwardScoreOnDestroyedTests
{
    // Builds an entity the way EntityFactory does: add the components, then start it.
    private static (Entity Enemy, Destroyable Destroyable) CreateEnemy(FakeScoreTracker score, bool increaseMultiplier = true)
    {
        var enemy = new Entity();
        var destroyable = enemy.AddComponent(new Destroyable());
        enemy.AddComponent(new AwardScoreOnDestroyed(score, 50, increaseMultiplier));
        enemy.Start();
        return (enemy, destroyable);
    }

    [Fact]
    public void Destroying_the_enemy_awards_its_points()
    {
        var score = new FakeScoreTracker();
        var (_, destroyable) = CreateEnemy(score);

        destroyable.Destroy();

        Assert.Equal([50], score.PointsAdded);
        Assert.Equal(1, score.MultiplierIncreases);
    }

    [Fact]
    public void Some_enemies_do_not_increase_the_multiplier()
    {
        var score = new FakeScoreTracker();
        var (_, destroyable) = CreateEnemy(score, increaseMultiplier: false);

        destroyable.Destroy();

        Assert.Equal([50], score.PointsAdded);
        Assert.Equal(0, score.MultiplierIncreases);
    }

    [Fact]
    public void An_enemy_destroyed_twice_only_scores_once()
    {
        var score = new FakeScoreTracker();
        var (_, destroyable) = CreateEnemy(score);

        destroyable.Destroy();
        destroyable.Destroy();

        Assert.Equal([50], score.PointsAdded);
    }

    [Fact]
    public void Nothing_is_awarded_before_the_enemy_is_destroyed()
    {
        var score = new FakeScoreTracker();
        CreateEnemy(score);

        Assert.Empty(score.PointsAdded);
    }
}
