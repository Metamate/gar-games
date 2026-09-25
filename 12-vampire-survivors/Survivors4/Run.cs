using System;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Survivors4;

// One run of the game: the player, the enemies, the weapons, the gems and the clock. The game
// states decide when it runs.
public sealed class Run(TextureAtlas atlas, Texture2D ground, Random random, Profiler profiler)
{
    public const float Duration = 300;  // survive five minutes
    private const int Capacity = 30000;

    public Player Player { get; } = new(atlas.GetRegion("hero"));
    public Swarm Swarm { get; } = new(atlas, Capacity);
    public Spawner Spawner { get; } = new(random);
    public BoltWeapon Bolts { get; } = new(atlas.GetRegion("bolt"));
    public Aura Aura { get; } = new(atlas.GetRegion("aura"));
    public Gems Gems { get; } = new(atlas.GetRegion("gem"), Capacity);

    public int Level { get; private set; } = 1;
    public int Experience { get; private set; }
    public int ExperienceToNext => 5 + Level * 5;
    public int Kills { get; private set; }

    public bool CanLevelUp => Experience >= ExperienceToNext;
    public bool IsWon => Spawner.Time >= Duration;

    public void LevelUp()
    {
        Experience -= ExperienceToNext;
        Level++;
    }

    public void Update(float deltaSeconds)
    {
        using (profiler.Measure("Player"))
            Player.Update(deltaSeconds);

        using (profiler.Measure("Spawn"))
        {
            int count = Spawner.Update(deltaSeconds);
            for (int i = 0; i < count; i++)
                Swarm.Spawn(Spawner.PickKind(), Spawner.PointAround(Player.Position));
        }

        using (profiler.Measure("Move"))
            Swarm.Move(deltaSeconds, Player.Position);

        using (profiler.Measure("Separate"))
            Swarm.Separate();

        using (profiler.Measure("Weapons"))
        {
            Bolts.Update(deltaSeconds, Swarm, Player.Position);
            Aura.Update(deltaSeconds, Swarm, Player.Position);
            Kills += Swarm.RemoveDead(Gems);
        }

        using (profiler.Measure("Gems"))
            Experience += Gems.Update(deltaSeconds, Player.Position);

        // Every enemy touching the player hurts, every second it touches.
        foreach (int i in Swarm.Within(Player.Position, Player.Radius))
            Player.Health -= EnemyKind.All[Swarm.Enemies.Kind[i]].Damage * deltaSeconds;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        int width = Game1.VirtualWidth, height = Game1.VirtualHeight;

        // The ground: one texture, repeated, and scrolled with the camera.
        spriteBatch.Begin(samplerState: SamplerState.LinearWrap);
        Rectangle view = new((int)Player.Position.X - width / 2, (int)Player.Position.Y - height / 2, width, height);
        spriteBatch.Draw(ground, new Rectangle(0, 0, width, height), view, Color.White);
        spriteBatch.End();

        // The world, with the camera centred on the player.
        Matrix camera = Matrix.CreateTranslation(width / 2 - Player.Position.X, height / 2 - Player.Position.Y, 0);
        spriteBatch.Begin(transformMatrix: camera);
        Aura.Draw(spriteBatch, Player.Position);
        Gems.Draw(spriteBatch);
        Swarm.Draw(spriteBatch);
        Bolts.Draw(spriteBatch);
        Player.Draw(spriteBatch);
        spriteBatch.End();
    }

    public void DrawHud(SpriteBatch spriteBatch, SpriteFont font)
    {
        int width = Game1.VirtualWidth, height = Game1.VirtualHeight;
        spriteBatch.Begin();

        // Experience along the top, health under the player.
        spriteBatch.Draw(Core.Pixel, new Rectangle(0, 0, width, 12), new Color(20, 20, 40));
        spriteBatch.Draw(Core.Pixel, new Rectangle(0, 0, width * Experience / ExperienceToNext, 12), new Color(60, 140, 255));
        spriteBatch.Draw(Core.Pixel, new Rectangle(width / 2 - 22, height / 2 + 30, 44, 6), new Color(60, 0, 0));
        spriteBatch.Draw(Core.Pixel, new Rectangle(width / 2 - 22, height / 2 + 30, (int)(44 * Math.Max(0, Player.Health) / Player.MaxHealth), 6), Color.Red);

        int seconds = (int)Spawner.Time;
        spriteBatch.DrawString(font, $"Level {Level}", new Vector2(20, 20), Color.White);
        string clock = $"{seconds / 60}:{seconds % 60:00} / 5:00";
        spriteBatch.DrawString(font, clock, new Vector2((width - font.MeasureString(clock).X) / 2, 20), Color.White);
        string kills = $"Kills {Kills}";
        spriteBatch.DrawString(font, kills, new Vector2(width - font.MeasureString(kills).X - 20, 20), Color.White);
        spriteBatch.End();
    }
}
