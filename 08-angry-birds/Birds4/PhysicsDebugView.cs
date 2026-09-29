using System;
using Birds4.Physics;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Birds4;

// Draws the physics bodies as outlines (F1), to see what the physics world sees: the ground,
// every box and circle, and which bodies are awake (green) or asleep (blue).
public class PhysicsDebugView(Texture2D pixel)
{
    public void Draw(SpriteBatch spriteBatch, PhysicsWorld world)
    {
        if (!DebugDraw.Enabled)
            return;

        foreach (PhysicsBody body in world.Bodies)
        {
            Color color = body.Type switch
            {
                BodyType.Static => Color.Orange,
                BodyType.Kinematic => Color.Magenta,
                _ => body.IsAwake ? Color.LimeGreen : Color.Blue,
            };
            if (body.IsCircle)
                DrawCircle(spriteBatch, body, color);
            else
                DrawBox(spriteBatch, body, color);
        }

        foreach (PhysicsJoint joint in world.Joints)
        {
            // A rope is a line between its anchors; a hinge or a weld is where the bodies meet.
            spriteBatch.DrawLine(pixel, joint.AnchorA, joint.AnchorB, Color.Yellow, 2);
            spriteBatch.Draw(pixel, new Rectangle((int)joint.AnchorA.X - 3, (int)joint.AnchorA.Y - 3, 6, 6), Color.Yellow);
        }
    }

    private void DrawBox(SpriteBatch spriteBatch, PhysicsBody body, Color color)
    {
        Vector2 half = body.Size / 2;
        Matrix turn = Matrix.CreateRotationZ(body.Rotation) * Matrix.CreateTranslation(body.Position.X, body.Position.Y, 0);
        Vector2[] corners =
        [
            Vector2.Transform(new Vector2(-half.X, -half.Y), turn),
            Vector2.Transform(new Vector2(half.X, -half.Y), turn),
            Vector2.Transform(new Vector2(half.X, half.Y), turn),
            Vector2.Transform(new Vector2(-half.X, half.Y), turn),
        ];
        for (int i = 0; i < 4; i++)
            spriteBatch.DrawLine(pixel, corners[i], corners[(i + 1) % 4], color, 2);
    }

    private void DrawCircle(SpriteBatch spriteBatch, PhysicsBody body, Color color)
    {
        const int Segments = 16;
        for (int i = 0; i < Segments; i++)
        {
            float a = MathHelper.TwoPi * i / Segments, b = MathHelper.TwoPi * (i + 1) / Segments;
            spriteBatch.DrawLine(pixel, body.Position + body.Radius * new Vector2(MathF.Cos(a), MathF.Sin(a)),
                body.Position + body.Radius * new Vector2(MathF.Cos(b), MathF.Sin(b)), color, 2);
        }

        // A line from the centre shows how the circle turns.
        spriteBatch.DrawLine(pixel, body.Position, body.Position + body.Radius * new Vector2(MathF.Cos(body.Rotation), MathF.Sin(body.Rotation)), color, 2);
    }
}
