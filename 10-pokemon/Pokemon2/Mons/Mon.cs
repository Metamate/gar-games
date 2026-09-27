namespace Pokemon2.Mons;

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
}
