using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace FlappyBird.Game.Elements
{
    public partial class Bird : CompositeDrawable
    {
        public double groundY = 0.0f;

        public bool isTouchingGround { get; set; }

        
        private TextureAnimation animation;

        [Resolved]
        private TextureStore textures { get; set; }

        private DrawableSample flapSound;

        private bool isIdle;

        private float currentVelocity;

        public Quad CollisionQuad
        {
            get
            {
                RectangleF rect = ScreenSpaceDrawQuad.AABBFloat;
                rect = rect.Shrink(new Vector2(rect.Width * 0.3f, rect.Height * 0.2f));
                return Quad.FromRectangle(rect);
            }
        }

        [BackgroundDependencyLoader]
        private void Load(ISampleStore samples)
        {
            Anchor = Anchor.CentreLeft;
            Origin = Anchor.Centre;
            Position = new Vector2(120.0f, 0.0f);

            animation = new ()
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
            };

            animation.AddFrame(textures.Get("redbird-upflap"), 100.0f);
            animation.AddFrame(textures.Get("redbird-downflap"), 100.0f);
            animation.AddFrame(textures.Get("redbird-midflap"), 100.0f);

            AddInternal(animation);
            AddInternal(flapSound = new (samples.Get("wing.ogg")));

            Size = animation.Size;
            Scale = new (3.5f);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Reset();
        }

        public void Reset()
        {
            isIdle = true;
            isTouchingGround = false;
            ClearTransforms();
            Rotation = 0.0f;
            Y = -60.0f;
            animation.IsPlaying = true;
            this.Loop(b => b.MoveToOffset(new Vector2(0.0f, -20.0f), 1000.0f, Easing.InOutSine)
                            .Then()
                            .MoveToOffset(new Vector2(0.0f, 20.0f), 1000.0f, Easing.InOutSine)
            );
        }

        public void FallDown()
        {
            currentVelocity = 0.0f;
            animation.IsPlaying = false;
            animation.GotoFrame(2);
        }

        public void FlyUp()
        {
            if (isIdle)
            {
                isIdle = false;
                ClearTransforms();
            }

            animation.GotoFrame(0);

            currentVelocity = 90.0f;
            this.RotateTo(-45.0f).Then(350.0f).RotateTo(90.0f, 500.0f);
            flapSound.Play();
        }

        protected override void Update()
        {
            if (isIdle)
            {
                base.Update();
                return;
            }

            currentVelocity -= (float)Clock.ElapsedFrameTime * 0.22f;
            Y -= currentVelocity * (float)Clock.ElapsedFrameTime * 0.01f;

            float groundPlane;
            if (groundY > 0.0f) groundPlane = (float)groundY / 2.0f;
            else groundPlane = Parent.DrawHeight - DrawHeight;

            Y = Math.Min(Y, groundPlane);

            if (Y >= groundPlane)
            {
                isTouchingGround = true;
                ClearTransforms();
            }

            base.Update();
        }
    }
}
