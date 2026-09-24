/// <summary>
/// Entry point for the content builder. When run, it builds the files in Content/Assets
/// according to the rules in GetContentCollection() below.
/// </summary>
/// <remarks>
/// Game projects run this automatically when they build (see BuildContent.targets).
/// For more details, see the MonoGame documentation:
///
///    https://docs.monogame.net/articles/getting_started/content_pipeline/content_builder_project.html
/// </remarks>

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using MonoGame.Framework.Content.Pipeline.Builder;

var builder = new Builder();

// The build arguments (platform, source and output folders) are passed in by the game project.
if (args.Length > 0)
    builder.Run(args);
else
    builder.Run(new ContentBuilderParams { Mode = ContentBuilderMode.None });

return builder.FailedToBuild > 0 ? -1 : 0;

public class Builder : ContentBuilder
{
    public override IContentCollection GetContentCollection()
    {
        var content = new ContentCollection();

        // Images are built into textures. Magenta pixels become transparent (color keying),
        // and mipmaps aren't needed for 2D pixel art.
        content.Include<WildcardRule>("*.png", new TextureImporter(), new TextureProcessor
        {
            ColorKeyEnabled = true,
            ColorKeyColor = Color.Magenta,
            GenerateMipmaps = false,
            PremultiplyAlpha = true,
        });

        // The atlas and tilemap definitions are read by our own code at runtime,
        // so they are copied as they are instead of being built.
        content.IncludeCopy<WildcardRule>("*.xml");

        return content;
    }
}
