using System.Collections.Generic;
using GeometryWars6.Systems;

namespace GeometryWars.Tests;

// A fake score tracker for tests: it only records what it was asked to do.
// AwardScoreOnDestroyed is given an IScoreTracker in its constructor, so a test can pass
// in this fake instead of the real ScoreTracker, and then check what the component did.
public sealed class FakeScoreTracker : IScoreTracker
{
    public List<int> PointsAdded { get; } = [];
    public int MultiplierIncreases { get; private set; }
    public int LivesRemoved { get; private set; }

    public bool IsGameOver => false;
    public void AddPoints(int basePoints) => PointsAdded.Add(basePoints);
    public void IncreaseMultiplier() => MultiplierIncreases++;
    public void RemoveLife() => LivesRemoved++;
}
