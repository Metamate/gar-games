using System;

namespace Survivors4;

// The choices when levelling up. Each one changes a number in the run.
public sealed record Upgrade(string Name, Action<Run> Apply)
{
    public static readonly Upgrade[] All =
    [
        new("Bolts: one more per volley", run => run.Bolts.Count++),
        new("Bolts: fire 20% faster", run => run.Bolts.Interval *= 0.8f),
        new("Bolts: +1 damage", run => run.Bolts.Damage += 1),
        new("Aura: 25% bigger", run => run.Aura.Radius *= 1.25f),
        new("Aura: +1 damage", run => run.Aura.Damage += 1),
        new("Boots: move 10% faster", run => run.Player.Speed *= 1.1f),
        new("Heal: +40 health", run => run.Player.Health = MathF.Min(run.Player.MaxHealth, run.Player.Health + 40)),
    ];
}
