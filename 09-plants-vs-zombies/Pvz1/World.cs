using System.Collections.Generic;
using System.Linq;
using Pvz1.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz1;

// Everything on the field, in one list per class.
public class World
{
    private readonly Defender[,] _defenders = new Defender[Field.Columns, Field.Rows];
    private readonly List<Goblin> _goblins = [];
    private readonly List<Arrow> _arrows = [];
    private readonly List<Coin> _coins = [];

    public int Gold { get; set; } = 150;
    public bool GoblinReachedCastle => _goblins.Any(goblin => goblin.Position.X < Field.Bounds.X);

    public IEnumerable<Defender> Defenders => _defenders.Cast<Defender>().Where(defender => defender != null);

    public bool IsFree(Point cell) => _defenders[cell.X, cell.Y] == null;
    public void AddDefender(Defender defender) => _defenders[defender.Cell.X, defender.Cell.Y] = defender;
    public void AddGoblin(Goblin goblin) => _goblins.Add(goblin);
    public void Add(Arrow arrow) => _arrows.Add(arrow);
    public void Add(Coin coin) => _coins.Add(coin);

    // The defender in this row at this x, if any.
    public Defender DefenderAt(int row, float x)
    {
        if (x < Field.Bounds.Left || x >= Field.Bounds.Right)
            return null;
        return _defenders[(int)(x - Field.Bounds.X) / Field.CellWidth, row];
    }

    // The nearest goblin on the field in this row, at or to the right of x.
    public Goblin FirstGoblinAhead(int row, float x)
        => _goblins.Where(goblin => goblin.Row == row && goblin.Position.X >= x && goblin.Position.X < Field.Bounds.Right + 20)
                   .OrderBy(goblin => goblin.Position.X)
                   .FirstOrDefault();

    public bool TryCollect(Vector2 point)
    {
        Coin coin = _coins.LastOrDefault(coin => coin.Contains(point));
        if (coin == null)
            return false;

        Gold += coin.Value;
        coin.IsGone = true;
        return true;
    }

    public void Update(float deltaSeconds)
    {
        // Loop over copies: defenders add arrows and coins while we loop.
        foreach (Defender defender in Defenders.ToList())
            defender.Update(deltaSeconds, this);
        foreach (Goblin goblin in _goblins.ToList())
            goblin.Update(deltaSeconds, this);
        foreach (Arrow arrow in _arrows.ToList())
            arrow.Update(deltaSeconds, this);
        foreach (Coin coin in _coins.ToList())
            coin.Update(deltaSeconds);

        foreach (Defender defender in Defenders.Where(defender => defender.IsDead).ToList())
            _defenders[defender.Cell.X, defender.Cell.Y] = null;
        _goblins.RemoveAll(goblin => goblin.IsDead);
        _arrows.RemoveAll(arrow => arrow.IsUsed);
        _coins.RemoveAll(coin => coin.IsGone);
    }

    // Row by row, from the back, so that nearer things are drawn on top.
    public void Draw(SpriteBatch spriteBatch)
    {
        for (int row = 0; row < Field.Rows; row++)
        {
            foreach (Defender defender in Defenders.Where(defender => defender.Row == row))
                defender.Draw(spriteBatch);
            foreach (Goblin goblin in _goblins.Where(goblin => goblin.Row == row))
                goblin.Draw(spriteBatch);
        }
        foreach (Arrow arrow in _arrows)
            arrow.Draw(spriteBatch);
        foreach (Coin coin in _coins)
            coin.Draw(spriteBatch);
    }
}
