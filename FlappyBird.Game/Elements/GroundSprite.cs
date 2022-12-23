using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace FlappyBird.Game.Elements;

public partial class GroundSprite : Sprite
{
    public GroundSprite()
    {
        Anchor = Anchor.BottomLeft;
        Origin = Anchor.BottomLeft;
    }

    [BackgroundDependencyLoader]
    private void Load(TextureStore textures)
    {
        Texture = textures.Get(name: "base");
        Scale = new Vector2(value: 2.5f);
        Position = new Vector2(x: 0.0f, y: 40.0f);
    }
}