using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pacman5.Routing;

// The arcade rule: take the direction whose next tile is closest to the target, in a straight
// line. It looks one tile ahead, so a wall can send the ghost the long way round.
public class NearestTile : IRouteStrategy
{
    public Point ChooseDirection(Ghost ghost, Point target, IReadOnlyList<Point> options)
    {
        Point best = options[0];
        float bestDistance = float.MaxValue;
        foreach (Point direction in options)
        {
            Point next = ghost.Tile + direction;
            float distance = Vector2.DistanceSquared(next.ToVector2(), target.ToVector2());
            if (distance < bestDistance)
            {
                best = direction;
                bestDistance = distance;
            }
        }
        return best;
    }
}
