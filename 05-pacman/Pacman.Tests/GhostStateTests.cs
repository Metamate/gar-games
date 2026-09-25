using System;
using Pacman4;
using Pacman4.GhostStates;
using Microsoft.Xna.Framework;
using Xunit;

namespace Pacman.Tests;

// The ghost states: each test puts a ghost in a state, makes something happen, and checks the
// state it ends up in. The game starts in the scatter phase, so Blinky starts in ScatterState.
public class GhostStateTests
{
    private readonly World _world = new(TestMaze.Text, new Random(1));

    private Ghost ChasingBlinky()
    {
        Ghost blinky = _world.Blinky;
        blinky.Place(new Point(5, 3), Direction.Right);
        blinky.ChangeState(new ChaseState());
        return blinky;
    }

    [Fact]
    public void Blinky_starts_outside_the_house_and_the_others_inside()
    {
        Assert.IsType<ScatterState>(_world.Blinky.State);
        Assert.IsType<InHouseState>(_world.Pinky.State);
    }

    [Fact]
    public void A_chasing_ghost_catches_Pac_Man()
    {
        Ghost blinky = ChasingBlinky();

        Assert.Equal(TouchResult.PacManCaught, blinky.Touch());
    }

    [Fact]
    public void A_power_pellet_frightens_a_chasing_ghost_and_turns_it_around()
    {
        Ghost blinky = ChasingBlinky();

        blinky.OnPowerPellet();

        Assert.IsType<FrightenedState>(blinky.State);
        Assert.Equal(Direction.Left, blinky.Heading);
    }

    [Fact]
    public void A_frightened_ghost_is_eaten_when_touched()
    {
        Ghost blinky = ChasingBlinky();
        blinky.OnPowerPellet();

        TouchResult result = blinky.Touch();

        Assert.Equal(TouchResult.GhostEaten, result);
        Assert.IsType<EatenState>(blinky.State);
    }

    [Fact]
    public void A_frightened_ghost_goes_back_to_the_schedule_when_time_runs_out()
    {
        Ghost blinky = ChasingBlinky();
        blinky.OnPowerPellet();

        blinky.Update(FrightenedState.Seconds + 0.1f);

        // The schedule is still in its first phase: scatter.
        Assert.IsType<ScatterState>(blinky.State);
    }

    [Fact]
    public void An_eaten_ghost_ignores_power_pellets_and_Pac_Man()
    {
        Ghost blinky = ChasingBlinky();
        blinky.ChangeState(new EatenState());

        blinky.OnPowerPellet();

        Assert.IsType<EatenState>(blinky.State);
        Assert.Equal(TouchResult.Nothing, blinky.Touch());
    }

    [Fact]
    public void An_eaten_ghost_returns_to_the_house_and_comes_back_out()
    {
        Ghost blinky = ChasingBlinky();
        blinky.ChangeState(new EatenState());

        // Small steps, as in the game, until the eyes reach the house.
        for (int i = 0; i < 120 && blinky.State is EatenState; i++)
            blinky.Update(1 / 60f);
        Assert.IsType<InHouseState>(blinky.State);

        for (int i = 0; i < 300 && blinky.State is InHouseState; i++)
            blinky.Update(1 / 60f);
        Assert.IsType<ScatterState>(blinky.State);
    }

    [Fact]
    public void A_phase_change_switches_from_chase_to_scatter_and_turns_the_ghost_around()
    {
        Ghost blinky = ChasingBlinky();

        blinky.OnPhaseChanged();

        Assert.IsType<ScatterState>(blinky.State);
        Assert.Equal(Direction.Left, blinky.Heading);
    }
}
