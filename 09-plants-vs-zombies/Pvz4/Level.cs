using System.Collections.Generic;

namespace Pvz4;

// A level, from level1.json: how much sun to start with, how often sun falls from the sky,
// and which zombie comes when.
public class Level
{
    public int StartingSun { get; init; }
    public float SkySunInterval { get; init; }
    public List<Spawn> Spawns { get; init; } = [];
}

public record Spawn(float Time, string Zombie);
