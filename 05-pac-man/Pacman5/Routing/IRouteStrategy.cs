using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pacman5.Routing;

// How a ghost gets to its target tile. At a tile centre, it picks one of the open directions.
// Targeting says where a ghost wants to go, and routing says which way it turns to get there.
public interface IRouteStrategy
{
    Point ChooseDirection(Ghost ghost, Point target, IReadOnlyList<Point> options);
}
