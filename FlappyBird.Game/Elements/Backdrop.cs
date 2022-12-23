using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace FlappyBird.Game.Elements;

public partial class Backdrop : CompositeDrawable
{
    private readonly Func<Sprite> createSprite;

    private Vector2 lastSize;

    private bool _running;
    public bool Running 
    { 
        get => _running; 
        set
        {
            if (value != Running) _running = value;
        }

    }

    public readonly float duration;

    public Backdrop(Func<Sprite> createSprite, float duration)
    {
        this.createSprite = createSprite;
        this.duration = duration;

    }

    [BackgroundDependencyLoader]
    private void Load()
    {
        RelativeSizeAxes = Axes.Both;
        Size = new Vector2(value: 1.0f);
        AddInternal(drawable: createSprite());

    }

    public void Start()
    {
        Running = true;
        UpdateLayout();
    }

    public void Freeze()
    {
        Running = false;
        StopAnimatingChildren();
    }

    protected override void UpdateAfterChildren()
    {
        base.UpdateAfterChildren();

        if (!DrawSize.Equals(other: lastSize))
        {
            UpdateLayout();
            lastSize = DrawSize;
        }
    }

    private void UpdateLayout()
    {
        float offset = -5.0f;
        Sprite? sprite = InternalChildren.First() as Sprite;

        var spriteNumber = Math.Ceiling(a: DrawWidth / sprite!.DrawWidth) + 1;

        if (spriteNumber != InternalChildren.Count)
        {
            while (InternalChildren.Count > spriteNumber) 
                RemoveInternal(drawable: InternalChildren.Last(), disposeImmediately: true);
            while (InternalChildren.Count < spriteNumber) 
                AddInternal(drawable: createSprite());
        }

        foreach (var childSprite in InternalChildren)
        {
            var width = childSprite.DrawWidth * sprite.Scale.X;
            childSprite.Position = new Vector2(x: offset, y: childSprite.Position.Y);

            var fromVector = new Vector2(x: offset, y: childSprite.Position.Y);
            var toVector = new Vector2(x: offset - width, y: childSprite.Position.Y);

            if (Running) childSprite.Loop(childGenerators: b => b.MoveTo(newPosition: fromVector)
                .MoveTo(newPosition: toVector, duration: duration));

            offset += width;
        }
    }

    private void StopAnimatingChildren() 
        => ClearTransforms(propagateChildren: true);
}