using System.Collections.Generic;
using System.Linq;
using Pvz3.Components;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz3;

// Everything on the lawn is an entity. The world keeps them by role (plants in the grid,
// zombies, and everything else), so that components can ask it questions.
public class World(TextureAtlas atlas)
{
    private readonly Entity[,] _plants = new Entity[Lawn.Columns, Lawn.Rows];
    private readonly List<Entity> _zombies = [];
    private readonly List<Entity> _others = [];
    private readonly List<Entity> _added = [];

    public TextureAtlas Atlas { get; } = atlas;
    public Recipes Recipes { get; } = new(atlas);
    public int Sun { get; set; } = 150;
    public bool ZombieReachedHouse => _zombies.Any(zombie => zombie.Position.X < Lawn.Bounds.X - 40);

    public IEnumerable<Entity> Plants => _plants.Cast<Entity>().Where(plant => plant != null);

    public bool IsFree(Point cell) => _plants[cell.X, cell.Y] == null;

    public void AddPlant(Entity plant, Point cell)
    {
        plant.Row = cell.Y;
        plant.Position = Lawn.CellFeet(cell);
        _plants[cell.X, cell.Y] = plant;
    }

    public void AddZombie(Entity zombie, int row)
    {
        zombie.Row = row;
        zombie.Position = new Vector2(Lawn.Bounds.Right + 60, Lawn.RowFeet(row));
        _zombies.Add(zombie);
    }

    // Peas, suns and effects. Added after the update, as they're often made during it.
    public void Add(Entity entity) => _added.Add(entity);

    // The plant in this row at this x, if any.
    public Entity PlantAt(int row, float x)
    {
        if (x < Lawn.Bounds.Left || x >= Lawn.Bounds.Right)
            return null;
        return _plants[(int)(x - Lawn.Bounds.X) / Lawn.CellWidth, row];
    }

    // The nearest zombie on the lawn in this row, at or to the right of x.
    public Entity FirstZombieAhead(int row, float x)
        => _zombies.Where(zombie => zombie.Row == row && zombie.Position.X >= x && zombie.Position.X < Lawn.Bounds.Right + 20)
                   .OrderBy(zombie => zombie.Position.X)
                   .FirstOrDefault();

    public IEnumerable<Entity> ZombiesWithin(Vector2 center, float radius)
        => _zombies.Where(zombie => Vector2.Distance(zombie.Position + new Vector2(0, -50), center) <= radius);

    public bool TryCollect(Vector2 point)
    {
        Collectible collectible = _others.Select(entity => entity.Get<Collectible>())
                                         .LastOrDefault(c => c != null && c.Contains(point));
        if (collectible == null)
            return false;

        Sun += collectible.Value;
        collectible.Owner.Remove();
        return true;
    }

    // One loop for every kind of entity: the world doesn't care what they are.
    public void Update(float deltaSeconds)
    {
        foreach (Entity entity in Plants.Concat(_zombies).Concat(_others).ToList())
            entity.Update(deltaSeconds);

        foreach (Entity plant in Plants.Where(plant => plant.IsRemoved).ToList())
        {
            Point cell = Lawn.CellAt(plant.Position - new Vector2(0, 1)).Value;
            _plants[cell.X, cell.Y] = null;
        }
        _zombies.RemoveAll(zombie => zombie.IsRemoved);
        _others.RemoveAll(entity => entity.IsRemoved);
        _others.AddRange(_added);
        _added.Clear();
    }

    // Row by row, from the back, so that nearer things are drawn on top.
    public void Draw(SpriteBatch spriteBatch)
    {
        for (int row = 0; row < Lawn.Rows; row++)
        {
            foreach (Entity plant in Plants.Where(plant => plant.Row == row))
                plant.Draw(spriteBatch);
            foreach (Entity zombie in _zombies.Where(zombie => zombie.Row == row))
                zombie.Draw(spriteBatch);
        }
        foreach (Entity entity in _others)
            entity.Draw(spriteBatch);
    }
}
