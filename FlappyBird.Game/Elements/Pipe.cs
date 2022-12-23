using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace FlappyBird.Game.Elements;

public partial class Pipe : Sprite
{
    public Pipe()
    {
        Anchor = Anchor.BottomCentre;
        Origin = Anchor.BottomCentre;
        Scale = new Vector2(value: 4.0f);
    }

    [BackgroundDependencyLoader]
    private void Load(TextureStore textures) 
        => Texture = textures.Get(name: "pipe-green");
    
}