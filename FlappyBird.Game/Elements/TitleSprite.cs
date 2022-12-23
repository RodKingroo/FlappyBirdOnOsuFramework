using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace FlappyBird.Game.Elements;

public partial class TitleSprite : Sprite
{
    private string textureName;
    
    private TextureStore? _textures;
    [Resolved]
    private TextureStore? Textures 
    { 
        get => _textures; 
        set
        {
            if (value != _textures) _textures = value;
        }
    }

    public TitleSprite(string textureName)
    {
        this.textureName = textureName;

        Anchor = Anchor.Centre;
        Origin = Anchor.Centre;
        Scale = new Vector2(value: 3.3f);
    }

    [BackgroundDependencyLoader]
    private void Load() => Texture = Textures?.Get(name: textureName);

}