using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Text;

namespace FlappyBird.Game.Elements;

public partial class ScoreSpriteText : SpriteText
{
    private ScoreGlyphStore? glyphStore;
    private TextureStore? _textures;

    [Resolved]
    private TextureStore Textures 
    { 
        get => _textures ?? throw new ArgumentNullException("Name is required"); 
        set 
        {
            if(value != _textures) _textures = value; 
        }
    }

    public ScoreSpriteText()
    {
        Text = "0";
        Anchor = Anchor.Centre;
        Origin = Anchor.Centre;

        RelativePositionAxes = Axes.Y;
        Y = -0.4f;
        
    }

    [BackgroundDependencyLoader]
    private void Load() => glyphStore = new ScoreGlyphStore(textures: Textures);

    protected override TextBuilder CreateTextBuilder(ITexturedGlyphLookupStore store)
        => base.CreateTextBuilder(store: glyphStore);
}