using osu.Framework.Allocation;
using osu.Framework.Extensions.PolygonExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osuTK;

namespace FlappyBird.Game.Elements
{
    public partial class PipeObstacle : CompositeDrawable
    {
        public float VerticalPositionAdjust = -130.0f;

        private Pipe topPipe;
        private Pipe bottomPipe;

        public PipeObstacle()
        {
            Anchor = Anchor.CentreLeft;
            Origin = Anchor.CentreLeft;

            RelativeSizeAxes = Axes.Y;
            AutoSizeAxes = Axes.X;
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            Random rnd = new ();
            topPipe = new ()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.TopCentre,
                Rotation = 180.0f,
                Position = new (0.0f, -(rnd.Next(75, 150)) + VerticalPositionAdjust),
            };

            topPipe.Scale = new (-topPipe.Scale.X, topPipe.Scale.Y);

            AddInternal(topPipe);

            bottomPipe = new ()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.TopCentre,
                Position = new (0.0f, 90 + VerticalPositionAdjust)
            };

            AddInternal(bottomPipe);
        }

        public bool CheckCollision(Quad birdQuad)
        {
            RectangleF topPipeRect = topPipe.ScreenSpaceDrawQuad.AABBFloat;
            topPipeRect.Y -= 5000.0f;
            topPipeRect.Height += 5000.0f;
            Quad topPipeQuad = Quad.FromRectangle(topPipeRect);

            if (birdQuad.Intersects(topPipeQuad)) return true;
            if (birdQuad.Intersects(bottomPipe.ScreenSpaceDrawQuad)) return true;

            return false;
        }
    }

}