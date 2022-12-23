using osu.Framework.Graphics.Textures;
using osu.Framework.Text;

namespace FlappyBird.Game.Elements;

public partial class ScoreSpriteText
{
    private class ScoreGlyphStore : ITexturedGlyphLookupStore
    {
        private readonly TextureStore textures;

        public ScoreGlyphStore(TextureStore textures) 
            => this.textures = textures;

        public ITexturedCharacterGlyph? Get(string fontName, char character)
        {
            Texture texture = textures.Get(name: $"{character}");

            return texture switch
            {
                null => null,
                _ => new TexturedCharacterGlyph(glyph: new CharacterGlyph(character: character, 
                xOffset: 0, yOffset: 0, xAdvance: texture.Width, baseline: 0, containingStore: null), 
                texture: texture, scale: 0.09f)
            };
        }

        public Task<ITexturedCharacterGlyph?> GetAsync(string fontName, char character) 
            => Task.Run(function: () => Get(fontName: fontName, character: character));
    }
}