using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace FlappyBird.Game.Elements
{
    public class GroundSprite : Sprite
    {
        public GroundSprite()
        {
            Anchor = Anchor.BottomLeft;
            Origin = Anchor.BottomLeft;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Texture = textures.Get("base");
            Scale = new Vector2(2.5f);
            Position = new Vector2(0.0f, 40.0f);
        }
    }
}