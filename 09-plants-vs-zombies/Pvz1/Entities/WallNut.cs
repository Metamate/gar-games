using GMDCore.Graphics;
using Microsoft.Xna.Framework;

namespace Pvz1.Entities;

// Does nothing but take a lot of bites.
public class WallNut(Point cell, TextureAtlas atlas) : Plant(cell, atlas.GetRegion("wall-nut"), 40)
{
}
