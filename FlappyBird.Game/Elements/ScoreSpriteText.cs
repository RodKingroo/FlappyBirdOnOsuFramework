using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Text;

namespace FlappyBird.Game.Elements
{
    public partial class ScoreSpriteText : SpriteText
    {
        private ScoreGlyphStore glyphStore;

        [Resolved]
        private TextureStore textures { get; set; }

        public ScoreSpriteText()
        {
            Text = "0";
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            RelativePositionAxes = Axes.Y;
            Y = -0.4f;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            glyphStore = new ScoreGlyphStore(textures);
        }

        protected override TextBuilder CreateTextBuilder(ITexturedGlyphLookupStore store) => base.CreateTextBuilder(glyphStore);

        private class ScoreGlyphStore : ITexturedGlyphLookupStore
        {
            private readonly TextureStore textures;

            public ScoreGlyphStore(TextureStore textures)
            {
                this.textures = textures;
            }

            public ITexturedCharacterGlyph Get(string fontName, char character)
            {
                var texture = textures.Get($"{character}");

                if (texture == null) return null;

                return new TexturedCharacterGlyph(new CharacterGlyph(character, 0, 0,
                    texture.Width, 0, null), texture, 0.09f);
            }

            public Task<ITexturedCharacterGlyph> GetAsync(string fontName, char character) => Task.Run(() => Get(fontName, character));
        }
    }
}