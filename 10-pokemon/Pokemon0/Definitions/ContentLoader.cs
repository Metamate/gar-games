using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Pokemon0.Definitions;

// Loads entity animations from a data file.
public static class ContentLoader
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static EntityAnimationsFile _animationsFile;

    public static void LoadContent(ContentManager content)
    {
        if (_animationsFile != null) return;

        // Load entity animation definitions
        string path = Path.Combine(content.RootDirectory, "data/entity_animations.json");
        using var stream = TitleContainer.OpenStream(path);
        _animationsFile = JsonSerializer.Deserialize<EntityAnimationsFile>(stream, JsonOptions);
    }

    // Build the walk + idle animation set from the shared entity atlas.
    public static Dictionary<string, Animation> CreateEntityAnimations(TextureAtlas atlas)
    {
        var animations = new Dictionary<string, Animation>();
        foreach (var entry in _animationsFile.Animations)
            animations[entry.Name] = atlas.CreateAnimation(entry.Frames, _animationsFile.Interval);
        return animations;
    }

    private record EntityAnimationsFile(double Interval, List<AnimationEntry> Animations);
    private record AnimationEntry(string Name, int[] Frames);
}
