using GARCore.Graphics;
using Microsoft.Xna.Framework;

namespace Pvz1.Entities;

// Does nothing but stand in the way, and take a lot of hits.
public class Knight(Point cell, TextureAtlas atlas) : Defender(cell, atlas.GetRegion("knight"), 40)
{
}
