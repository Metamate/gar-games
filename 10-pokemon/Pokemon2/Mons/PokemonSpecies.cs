namespace Pokemon2.Mons;

// Blueprint for a Mon species: base stats and growth values, shared by every monster of that species.
public sealed record PokemonSpecies(
    string Name,
    string BattleSpriteFront,
    string BattleSpriteBack,
    // Base stats (before level scaling)
    int BaseHp,
    int BaseAttack,
    int BaseDefense,
    int BaseSpeed,
    // Growth values (1–5): how fast each stat grows with the level
    int HpGrowth,
    int AttackGrowth,
    int DefenseGrowth,
    int SpeedGrowth);
