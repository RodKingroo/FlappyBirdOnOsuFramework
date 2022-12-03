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

        public bool Running { get; private set; }

        public readonly double Duration;

        public Backdrop(Func<Sprite> createSprite, double duration = 2000.0f)
        {
            this.createSprite = createSprite;
            Duration = duration;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Size = new Vector2(1.0f);

            AddInternal(createSprite());
        }

        public void Start()
        {
            if (Running) return;

            Running = true;
            updateLayout();
        }

        public void Freeze()
        {
            if (!Running)
                return;

            Running = false;
            stopAnimatingChildren();
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            if (!DrawSize.Equals(lastSize))
            {
                updateLayout();
                lastSize = DrawSize;
            }
        }

        private void updateLayout()
        {
            Sprite sprite = (Sprite)InternalChildren.First();

            var spriteNum = (int)Math.Ceiling(DrawWidth / sprite.DrawWidth) + 1;

            if (spriteNum != InternalChildren.Count)
            {
                while (InternalChildren.Count > spriteNum) RemoveInternal(InternalChildren.Last(), true);

                while (InternalChildren.Count < spriteNum) AddInternal(createSprite());
            }

            var offset = 0.0f;

            foreach (var childSprite in InternalChildren)
            {
                var width = childSprite.DrawWidth * sprite.Scale.X;
                childSprite.Position = new Vector2(offset, childSprite.Position.Y);

                var fromVector = new Vector2(offset, childSprite.Position.Y);
                var toVector = new Vector2(offset - width * 1.5f, childSprite.Position.Y);

                if (Running) childSprite.Loop(b => b.MoveTo(fromVector).MoveTo(toVector, Duration));

                offset += width;
            }
        }

        private void stopAnimatingChildren() => ClearTransforms(true);
    }
}