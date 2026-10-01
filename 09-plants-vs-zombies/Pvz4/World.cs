using System.Collections.Generic;
using System.Linq;
using Pvz4.Components;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz4;

// Everything on the field is an entity. The world keeps them by role (defenders in the grid,
// goblins, and everything else), so that components can ask it questions.
public class World(TextureAtlas atlas)
{
    private readonly Entity[,] _defenders = new Entity[Field.Columns, Field.Rows];
    private readonly List<Entity> _goblins = [];
    private readonly List<Entity> _others = [];
    private readonly List<Entity> _added = [];

    public TextureAtlas Atlas { get; } = atlas;
    public Recipes Recipes { get; } = new(atlas);
    public int Gold { get; set; } = 150;
    public bool GoblinReachedCastle => _goblins.Any(goblin => goblin.Position.X < Field.Bounds.X);
    public int GoblinCount => _goblins.Count;

    public IEnumerable<Entity> Defenders => _defenders.Cast<Entity>().Where(defender => defender != null);

    public bool IsFree(Point cell) => _defenders[cell.X, cell.Y] == null;

    public void AddDefender(Entity defender, Point cell)
    {
        defender.Row = cell.Y;
        defender.Position = Field.CellFeet(cell);
        _defenders[cell.X, cell.Y] = defender;
    }

    public void AddGoblin(Entity goblin, int row)
    {
        goblin.Row = row;
        goblin.Position = new Vector2(Field.Bounds.Right + 120, Field.RowFeet(row));
        _goblins.Add(goblin);
    }

    // Arrows, coins and effects. Added after the update, as they're often made during it.
    public void Add(Entity entity) => _added.Add(entity);

    // The defender in this row at this x, if any.
    public Entity DefenderAt(int row, float x)
    {
        if (x < Field.Bounds.Left || x >= Field.Bounds.Right)
            return null;
        return _defenders[(int)(x - Field.Bounds.X) / Field.CellWidth, row];
    }

    // The nearest goblin on the field in this row, at or to the right of x.
    public Entity FirstGoblinAhead(int row, float x)
        => _goblins.Where(goblin => goblin.Row == row && goblin.Position.X >= x && goblin.Position.X < Field.Bounds.Right + 20)
                   .OrderBy(goblin => goblin.Position.X)
                   .FirstOrDefault();

    public IEnumerable<Entity> GoblinsWithin(Vector2 center, float radius)
        => _goblins.Where(goblin => Vector2.Distance(goblin.Position + new Vector2(0, -40), center) <= radius);

    public bool TryCollect(Vector2 point)
    {
        Collectible collectible = _others.Select(entity => entity.Get<Collectible>())
                                         .LastOrDefault(c => c != null && c.Contains(point));
        if (collectible == null)
            return false;

        Gold += collectible.Value;
        collectible.Owner.Remove();
        return true;
    }

    // One loop for every kind of entity: the world doesn't care what they are.
    public void Update(float deltaSeconds)
    {
        foreach (Entity entity in Defenders.Concat(_goblins).Concat(_others).ToList())
            entity.Update(deltaSeconds);

        foreach (Entity defender in Defenders.Where(defender => defender.IsRemoved).ToList())
        {
            Point cell = Field.CellAt(defender.Position - new Vector2(0, 1)).Value;
            _defenders[cell.X, cell.Y] = null;
        }
        _goblins.RemoveAll(goblin => goblin.IsRemoved);
        _others.RemoveAll(entity => entity.IsRemoved);
        _others.AddRange(_added);
        _added.Clear();
    }

    // Row by row, from the back, so that nearer things are drawn on top.
    public void Draw(SpriteBatch spriteBatch)
    {
        for (int row = 0; row < Field.Rows; row++)
        {
            foreach (Entity defender in Defenders.Where(defender => defender.Row == row))
                defender.Draw(spriteBatch);
            foreach (Entity goblin in _goblins.Where(goblin => goblin.Row == row))
                goblin.Draw(spriteBatch);
        }
        foreach (Entity entity in _others)
            entity.Draw(spriteBatch);
    }
}
