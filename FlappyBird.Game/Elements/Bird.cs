using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace FlappyBird.Game.Elements;

public partial class Bird : CompositeDrawable
{
    public double groundY = 0.0f;

    private bool _isTouchingGround;
    public bool IsTouchingGround
    {
        get => _isTouchingGround;
        set
        {
            if (value != _isTouchingGround) _isTouchingGround = value;
        }
    }


    private TextureAnimation? animation;
    private TextureStore? _textures;
    [Resolved]
    private TextureStore Textures 
    { 
        get => _textures ?? throw new ArgumentNullException("Name is required"); 
        set
        {
            if(value != _textures) _textures = value; 
        } 
    
    }

    private DrawableSample? flapSound;

    private bool isIdle;

    private float currentVelocity;
    

    public Quad CollisionQuad
    {
        get
        {
            var rect = ScreenSpaceDrawQuad.AABBFloat;
            rect = rect.Shrink(amount: new Vector2(x: rect.Width, y: rect.Height));
            return Quad.FromRectangle(rectangle: rect);
        }
    }

    [BackgroundDependencyLoader]
    private void Load(ISampleStore samples)
    {
        Anchor = Anchor.CentreLeft;
        Origin = Anchor.Centre;
        Position = new Vector2(x: 120.0f, y: 0.0f);

        animation = new()
        {
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
        };

        animation.AddFrame(content: Textures.Get(name: "redbird-upflap"), displayDuration: 100.0f);
        animation.AddFrame(content: Textures.Get(name: "redbird-downflap"), displayDuration: 100.0f);
        animation.AddFrame(content: Textures.Get(name: "redbird-midflap"), displayDuration: 100.0f);

        AddInternal(drawable: animation);
        AddInternal(drawable: flapSound = new(sample: samples.Get(name: "wing.ogg")));

        Size = animation.Size;
        Scale = new Vector2(value: 3.5f);
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        Reset();
    }

    public void Reset()
    {
        isIdle = true;
        IsTouchingGround = false;
        ClearTransforms();
        Rotation = 0.0f;
        Y = -60.0f;
        animation!.IsPlaying = true;
        this.Loop(childGenerators: b 
            => b.MoveToOffset(new Vector2(0.0f, -20.0f), 1000.0f, Easing.InOutSine)
                        .Then()
                        .MoveToOffset(new Vector2(0.0f, 20.0f), 1000.0f, Easing.InOutSine)
        );
    }

    public void FallDown()
    {
        currentVelocity = 0.0f;
        animation!.IsPlaying = false;
        animation.GotoFrame(2);
    }

    public void FlyUp()
    {
        if (isIdle)
        {
            isIdle = false;
            ClearTransforms();
        }

        animation!.GotoFrame(0);

        currentVelocity = 90.0f;
        this.RotateTo(newRotation: -45.0f).Then(delay: 350.0f)
            .RotateTo(newRotation: 90.0f, duration: 500.0f);
        flapSound!.Play();
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

        Y = Math.Min(val1: Y, groundPlane);

        if (Y >= groundPlane)
        {
            IsTouchingGround = true;
            ClearTransforms();
        }

        base.Update();
    }
}
