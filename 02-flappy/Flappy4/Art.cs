using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy4;

public static class Art
{
    public static Texture2D Bird { get; set; }
    public static Texture2D Background { get; set; }
    public static Texture2D Ground { get; set; }

    public static void LoadContent(ContentManager content)
    {
        Bird = content.Load<Texture2D>("images/bird");
        Background = content.Load<Texture2D>("images/background");
        Ground = content.Load<Texture2D>("images/ground");
    }
}