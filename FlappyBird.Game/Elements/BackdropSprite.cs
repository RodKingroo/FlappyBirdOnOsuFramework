using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace FlappyBird.Game.Elements;

public partial class BackdropSprite : Sprite
{
    public BackdropSprite()
    {
        Anchor = Anchor.TopLeft;
        Origin = Anchor.TopLeft;
    }

    [BackgroundDependencyLoader]
    private void Load(TextureStore textures)
    {
        Texture = textures.Get(name: "background-day");
        RelativeSizeAxes = Axes.Y;
        Height = 1.0f;
    }

    protected override void Update()
    {
        base.Update();
        Width = (float)Math.Ceiling(a: DrawHeight * (double)(Texture.Size.X / Texture.Size.Y));
    }
}