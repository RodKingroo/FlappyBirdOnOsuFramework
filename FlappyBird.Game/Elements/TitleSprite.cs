using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace FlappyBird.Game.Elements
{
    public class TitleSprite : Sprite
    {
        private string textureName;

        [Resolved]
        private TextureStore textures { get; set; }

        public TitleSprite(string textureName)
        {
            this.textureName = textureName;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Scale = new Vector2(3.3f);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Texture = textures.Get(textureName);
        }
    }
}