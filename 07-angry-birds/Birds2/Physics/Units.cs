using Box2D.NET;
using Microsoft.Xna.Framework;

namespace Birds2.Physics;

// Box2D works in metres, with y pointing up. The game works in pixels, with y pointing down.
// Every conversion between the two happens here, and nowhere else.
internal static class Units
{
    public const float PixelsPerMeter = 50;

    public static float ToMeters(float pixels) => pixels / PixelsPerMeter;
    public static float ToPixels(float meters) => meters * PixelsPerMeter;

    public static B2Vec2 ToMeters(Vector2 pixels) => new(pixels.X / PixelsPerMeter, -pixels.Y / PixelsPerMeter);
    public static Vector2 ToPixels(B2Vec2 meters) => new(meters.X * PixelsPerMeter, -meters.Y * PixelsPerMeter);
}
