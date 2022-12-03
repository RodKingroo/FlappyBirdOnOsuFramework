using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace FlappyBird.Game.Elements
{
    public partial class Pipe : Sprite
    {
        public Pipe()
        {
            Anchor = Anchor.BottomCentre;
            Origin = Anchor.BottomCentre;
            Scale = new Vector2(4.1f);
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Texture = textures.Get("pipe-green");
        }
    }
}