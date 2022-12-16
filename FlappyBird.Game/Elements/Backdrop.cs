using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace FlappyBird.Game.Elements
{
    public partial class Backdrop : CompositeDrawable
    {
        private readonly Func<Sprite> createSprite;

        private Vector2 lastSize;

        public bool running { get; set; }

        public readonly double duration;

        public Backdrop(Func<Sprite> createSprite, double duration = 2000.0)
        {
            this.createSprite = createSprite;
            this.duration = duration;
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.Both;
            Size = new (1.0f);
            AddInternal(createSprite());

        }

        public void Start()
        {
            if (running) return;

            running = true;
            UpdateLayout();
        }

        public void Freeze()
        {
            if (!running) return;

            running = false;
            StopAnimatingChildren();
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            if (!DrawSize.Equals(lastSize))
            {
                UpdateLayout();
                lastSize = DrawSize;
            }
        }

        private void UpdateLayout()
        {
            Sprite sprite = (Sprite)InternalChildren.First();

            double spriteNum = Math.Ceiling(DrawWidth / sprite.DrawWidth) + 1;

            if (spriteNum != InternalChildren.Count)
            {
                while (InternalChildren.Count > spriteNum) RemoveInternal(InternalChildren.Last(), true);

                while (InternalChildren.Count < spriteNum) AddInternal(createSprite());
            }

            float offset = 0.0f;

            foreach (Drawable? childSprite in InternalChildren)
            {
                float width = childSprite.DrawWidth * sprite.Scale.X;
                childSprite.Position = new Vector2(offset, childSprite.Position.Y);

                Vector2 fromVector = new (offset, childSprite.Position.Y);
                Vector2 toVector = new (offset - width * 1.5f, childSprite.Position.Y);

                if (running) childSprite.Loop(b => b.MoveTo(fromVector).MoveTo(toVector, duration));

                offset += width;
            }
        }

        private void StopAnimatingChildren() => ClearTransforms(true);
    }
}