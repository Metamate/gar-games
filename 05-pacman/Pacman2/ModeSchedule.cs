namespace Pacman2;

public enum Phase { Scatter, Chase }

// The ghosts take turns: a few seconds scattering to their corners, then a longer chase. The
// schedule is the same for all four ghosts, so it lives outside them.
public class ModeSchedule
{
    private static readonly (Phase Phase, float Seconds)[] Phases =
    [
        (Phase.Scatter, 7), (Phase.Chase, 20),
        (Phase.Scatter, 7), (Phase.Chase, 20),
        (Phase.Scatter, 5), (Phase.Chase, 20),
        (Phase.Scatter, 5), (Phase.Chase, float.PositiveInfinity),
    ];

    private int _index;
    private float _elapsed;

    public Phase Current => Phases[_index].Phase;

    // Returns true when the phase changes.
    public bool Update(float deltaSeconds)
    {
        _elapsed += deltaSeconds;
        if (_elapsed < Phases[_index].Seconds)
            return false;

        _elapsed -= Phases[_index].Seconds;
        _index++;
        return true;
    }

    public void Reset()
    {
        _index = 0;
        _elapsed = 0;
    }
}
