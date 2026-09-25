using System;
using System.IO;
using GeometryWars6;
using GeometryWars6.Services;
using GeometryWars6.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xunit;

namespace GeometryWars.Tests;

// ScoreTracker gets the frame time through FrameInfo, passed into its constructor. So a test
// can decide how much time passes, instead of waiting for it.
public class ScoreTrackerTests
{
    private readonly FrameInfo _frame = new();

    private ScoreTracker CreateTracker()
    {
        // A high-score file that doesn't exist yet, so every test starts from zero.
        string path = Path.Combine(Path.GetTempPath(), "gar-tests", Guid.NewGuid() + ".txt");
        var tracker = new ScoreTracker(_frame, path);
        tracker.StartNewRun();
        return tracker;
    }

    private void PassTime(ScoreTracker tracker, float seconds)
    {
        _frame.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(seconds)), new Viewport());
        tracker.Update();
    }

    [Fact]
    public void Points_are_multiplied_by_the_multiplier()
    {
        ScoreTracker tracker = CreateTracker();
        tracker.IncreaseMultiplier();
        tracker.IncreaseMultiplier();

        tracker.AddPoints(10);

        Assert.Equal(3, tracker.Multiplier);
        Assert.Equal(30, tracker.Score);
    }

    [Fact]
    public void The_multiplier_stops_at_its_maximum()
    {
        ScoreTracker tracker = CreateTracker();

        for (int i = 0; i < 100; i++)
            tracker.IncreaseMultiplier();

        Assert.Equal(GameSettings.Player.MaxMultiplier, tracker.Multiplier);
    }

    [Fact]
    public void The_multiplier_resets_when_no_enemy_is_killed_for_a_while()
    {
        ScoreTracker tracker = CreateTracker();
        tracker.IncreaseMultiplier();

        PassTime(tracker, GameSettings.Player.MultiplierExpiry / 2);
        Assert.Equal(2, tracker.Multiplier);

        PassTime(tracker, GameSettings.Player.MultiplierExpiry);
        Assert.Equal(1, tracker.Multiplier);
    }

    [Fact]
    public void No_points_are_scored_after_game_over()
    {
        ScoreTracker tracker = CreateTracker();
        for (int i = 0; i < GameSettings.Player.StartingLives; i++)
            tracker.RemoveLife();

        tracker.AddPoints(10);

        Assert.True(tracker.IsGameOver);
        Assert.Equal(0, tracker.Score);
    }
}
