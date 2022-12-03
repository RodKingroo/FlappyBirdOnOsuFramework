using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace FlappyBird.Game.Elements
{
    public class ScreenFlash : Box
    {
        public ScreenFlash()
        {
            Colour = Colour4.White;
            RelativeSizeAxes = Axes.Both;
            Alpha = 0.0f;

        }

        public void Flash(double fadeInDuration, double fadeOutDuration)
        {
            this.FadeIn(fadeInDuration).Then().FadeOut(fadeOutDuration);
            
        }
    }
}