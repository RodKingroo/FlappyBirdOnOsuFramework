using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace FlappyBird.Game.Elements
{
    public partial class TitleSprite : Sprite
    {
        private string textureName;

        [Resolved]
        private TextureStore? textures { get; set; }

        public TitleSprite(string textureName)
        {
            this.textureName = textureName;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Scale = new (3.3f);
        }

        [BackgroundDependencyLoader]
        private void Load() => Texture = textures?.Get(textureName);

    }
}