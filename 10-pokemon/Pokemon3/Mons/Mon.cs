using System;

namespace Pokemon3.Mons;

// A runtime Pokemon instance: a species at a level. Its stats follow from both.
public sealed class Mon
{
    private readonly PokemonSpecies _species;

    public string Name              => _species.Name;
    public string BattleSpriteFront => _species.BattleSpriteFront;
    public string BattleSpriteBack  => _species.BattleSpriteBack;

    // Stats: the base stat plus half the growth value per level. No dice: the same
    // species at the same level always has the same stats.
    public int Hp      => StatAt(_species.BaseHp,      _species.HpGrowth);
    public int Attack  => StatAt(_species.BaseAttack,  _species.AttackGrowth);
    public int Defense => StatAt(_species.BaseDefense, _species.DefenseGrowth);
    public int Speed   => StatAt(_species.BaseSpeed,   _species.SpeedGrowth);

    public int Level       { get; private set; }
    public int CurrentHp   { get; set; }
    public int CurrentExp  { get; set; }

    // Exp needed for the next level: 10 per level.
    public int ExpToLevel => Level * 10;

    public Mon(PokemonSpecies species, int level)
    {
        _species  = species;
        Level     = level;
        CurrentHp = Hp;
    }

    private int StatAt(int baseStat, int growth) => baseStat + growth * Level / 2;

    // Advance one level. Returns the stat gains, for the level-up message.
    public (int hpGain, int atkGain, int defGain, int spdGain) LevelUp()
    {
        var (hp, atk, def, spd) = (Hp, Attack, Defense, Speed);
        Level++;
        return (Hp - hp, Attack - atk, Defense - def, Speed - spd);
    }
    // Restore HP to full.
    public void Heal() => CurrentHp = Hp;

    // Damage this Pokemon deals to a defender using a specific move.
    public int CalcDamageTo(Mon defender, Move move)
    {
        // Simple damage formula: (Attack * MovePower / 10) - Defense, minimum 1
        int damage = (Attack * move.BasePower / 10) - defender.Defense;
        return Math.Max(1, damage);
    }

    // Exp for beating this Pokemon: half a level's worth, at the same level.
    public int ExpReward => Level * 5;
}
