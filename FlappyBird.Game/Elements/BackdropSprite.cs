using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace FlappyBird.Game.Elements
{
    public class BackdropSprite : Sprite
    {
        public BackdropSprite()
        {
            Anchor = Anchor.TopLeft;
            Origin = Anchor.TopLeft;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Texture = textures.Get("background-day");
            RelativeSizeAxes = Axes.Y;
            Height = 1.0f;
        }

        protected override void Update()
        {
            base.Update();

            Vector2 size = Texture.Size;
            double aspectRatio = size.X / size.Y;

            Width = (float)Math.Ceiling(DrawHeight * aspectRatio);
        }
    }
}