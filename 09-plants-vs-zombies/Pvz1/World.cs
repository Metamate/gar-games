using System.Collections.Generic;
using System.Linq;
using Pvz1.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz1;

// Everything on the lawn, in one list per class.
public class World
{
    private readonly Plant[,] _plants = new Plant[Lawn.Columns, Lawn.Rows];
    private readonly List<Zombie> _zombies = [];
    private readonly List<Pea> _peas = [];
    private readonly List<Sun> _suns = [];

    public int Sun { get; set; } = 150;
    public bool ZombieReachedHouse => _zombies.Any(zombie => zombie.Position.X < Lawn.Bounds.X - 40);

    public IEnumerable<Plant> Plants => _plants.Cast<Plant>().Where(plant => plant != null);

    public bool IsFree(Point cell) => _plants[cell.X, cell.Y] == null;
    public void AddPlant(Plant plant) => _plants[plant.Cell.X, plant.Cell.Y] = plant;
    public void AddZombie(Zombie zombie) => _zombies.Add(zombie);
    public void Add(Pea pea) => _peas.Add(pea);
    public void Add(Sun sun) => _suns.Add(sun);

    // The plant in this row at this x, if any.
    public Plant PlantAt(int row, float x)
    {
        if (x < Lawn.Bounds.Left || x >= Lawn.Bounds.Right)
            return null;
        return _plants[(int)(x - Lawn.Bounds.X) / Lawn.CellWidth, row];
    }

    // The nearest zombie on the lawn in this row, at or to the right of x.
    public Zombie FirstZombieAhead(int row, float x)
        => _zombies.Where(zombie => zombie.Row == row && zombie.Position.X >= x && zombie.Position.X < Lawn.Bounds.Right + 20)
                   .OrderBy(zombie => zombie.Position.X)
                   .FirstOrDefault();

    public bool TryCollect(Vector2 point)
    {
        Sun sun = _suns.LastOrDefault(sun => sun.Contains(point));
        if (sun == null)
            return false;

        Sun += sun.Value;
        sun.IsGone = true;
        return true;
    }

    public void Update(float deltaSeconds)
    {
        // Loop over copies: plants add peas and suns while we loop.
        foreach (Plant plant in Plants.ToList())
            plant.Update(deltaSeconds, this);
        foreach (Zombie zombie in _zombies.ToList())
            zombie.Update(deltaSeconds, this);
        foreach (Pea pea in _peas.ToList())
            pea.Update(deltaSeconds, this);
        foreach (Sun sun in _suns.ToList())
            sun.Update(deltaSeconds);

        foreach (Plant plant in Plants.Where(plant => plant.IsDead).ToList())
            _plants[plant.Cell.X, plant.Cell.Y] = null;
        _zombies.RemoveAll(zombie => zombie.IsDead);
        _peas.RemoveAll(pea => pea.IsUsed);
        _suns.RemoveAll(sun => sun.IsGone);
    }

    // Row by row, from the back, so that nearer things are drawn on top.
    public void Draw(SpriteBatch spriteBatch)
    {
        for (int row = 0; row < Lawn.Rows; row++)
        {
            foreach (Plant plant in Plants.Where(plant => plant.Row == row))
                plant.Draw(spriteBatch);
            foreach (Zombie zombie in _zombies.Where(zombie => zombie.Row == row))
                zombie.Draw(spriteBatch);
        }
        foreach (Pea pea in _peas)
            pea.Draw(spriteBatch);
        foreach (Sun sun in _suns)
            sun.Draw(spriteBatch);
    }
}
