using osu.Framework.Allocation;
using osu.Framework.Extensions.PolygonExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osuTK;

namespace FlappyBird.Game.Elements;

public partial class PipeObstacle : CompositeDrawable
{
    public float verticalPositionAdjust = -130.0f;

    private Pipe? topPipe;
    private Pipe? bottomPipe;

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
        topPipe = new Pipe()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.TopCentre,
            Rotation = 180.0f,
            Position = new Vector2(x: 0.0f, y: -120 + verticalPositionAdjust),
        };

        topPipe.Scale = new Vector2(x: -topPipe.Scale.X * 1.5f, y: topPipe.Scale.Y);

        AddInternal(drawable: topPipe);

        bottomPipe = new Pipe()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.TopCentre,
            Position = new Vector2(x: 0.0f, y: 120 + verticalPositionAdjust)
        };

        bottomPipe.Scale = new Vector2(x: -bottomPipe.Scale.X * 1.5f, y: bottomPipe.Scale.Y);

        AddInternal(drawable: bottomPipe);
    }

    public bool CheckCollision(Quad birdQuad)
    {
        var topPipeRect = topPipe!.ScreenSpaceDrawQuad.AABBFloat;
        topPipeRect.Y -= 5000.0f;
        topPipeRect.Height += 5000.0f;
        
        if (birdQuad.Intersects(second: Quad.FromRectangle(rectangle: topPipeRect))) return true;
        if (birdQuad.Intersects(second: bottomPipe!.ScreenSpaceDrawQuad)) return true;

        return false;
    }
}