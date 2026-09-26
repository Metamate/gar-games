using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Survivors3;

// Measures how long each part of the game takes, averaged over the last second, and how much
// memory it allocates. F3 shows it.
//
//   using (_profiler.Measure("Separate"))
//       _swarm.Separate();
public sealed class Profiler
{
    private readonly List<string> _names = [];
    private readonly Dictionary<string, long> _ticks = [];
    private readonly Dictionary<string, double> _milliseconds = [];
    private int _frames;
    private float _elapsed;
    private int _framesPerSecond;
    private long _allocatedAtStart = GC.GetAllocatedBytesForCurrentThread();
    private long _allocatedPerFrame;
    private int _collectionsAtStart = GC.CollectionCount(0);
    private int _collectionsPerSecond;

    public bool IsVisible { get; set; } = true;

    public Scope Measure(string name) => new(this, name, Stopwatch.GetTimestamp());

    // A struct, so that measuring doesn't allocate anything itself.
    public readonly struct Scope(Profiler profiler, string name, long start) : IDisposable
    {
        public void Dispose() => profiler.Add(name, Stopwatch.GetTimestamp() - start);
    }

    private void Add(string name, long ticks)
    {
        if (!_ticks.ContainsKey(name))
        {
            _names.Add(name);
            _ticks[name] = 0;
            _milliseconds[name] = 0;
        }
        _ticks[name] += ticks;
    }

    // Call once per drawn frame.
    public void EndFrame(float deltaSeconds)
    {
        _frames++;
        _elapsed += deltaSeconds;
        if (_elapsed < 1)
            return;

        foreach (string name in _names)
        {
            _milliseconds[name] = _ticks[name] * 1000.0 / Stopwatch.Frequency / _frames;
            _ticks[name] = 0;
        }

        long allocated = GC.GetAllocatedBytesForCurrentThread();
        _allocatedPerFrame = (allocated - _allocatedAtStart) / _frames;
        _allocatedAtStart = allocated;
        int collections = GC.CollectionCount(0);
        _collectionsPerSecond = collections - _collectionsAtStart;
        _collectionsAtStart = collections;

        _framesPerSecond = (int)MathF.Round(_frames / _elapsed);
        _frames = 0;
        _elapsed = 0;
    }

    public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 position, int enemies)
    {
        if (!IsVisible)
            return;

        var lines = new List<string> { $"FPS {_framesPerSecond,4}   enemies {enemies,6}" };
        double total = 0;
        foreach (string name in _names)
        {
            lines.Add($"{name,-12}{_milliseconds[name],7:F2} ms");
            total += _milliseconds[name];
        }
        lines.Add($"{"Total",-12}{total,7:F2} ms   (a frame at 60 FPS: 16.7 ms)");
        lines.Add($"Allocated {_allocatedPerFrame / 1024.0,8:F1} KB per frame");
        lines.Add($"Gen 0 GCs {_collectionsPerSecond,8} per second");

        float height = lines.Count * font.LineSpacing + 12;
        spriteBatch.Draw(GMDCore.Core.Pixel, new Rectangle((int)position.X - 8, (int)position.Y - 6, 480, (int)height), Color.Black * 0.6f);
        for (int i = 0; i < lines.Count; i++)
            spriteBatch.DrawString(font, lines[i], position + new Vector2(0, i * font.LineSpacing), Color.Yellow);
    }
}
