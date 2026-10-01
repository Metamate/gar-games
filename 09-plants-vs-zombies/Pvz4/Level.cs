using System.Collections.Generic;

namespace Pvz4;

// A level, from level1.json: how much gold to start with, how often gold falls from the sky,
// and which goblin comes when.
public class Level
{
    public int StartingGold { get; init; }
    public float SkyGoldInterval { get; init; }
    public List<Spawn> Spawns { get; init; } = [];
}

public record Spawn(float Time, string Goblin);
