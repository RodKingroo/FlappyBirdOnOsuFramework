using System.Diagnostics;
using FlappyBird.Game.Elements;
using FlappyBird.Resources;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.IO.Stores;
using osu.Framework.Audio.Sample;
using osuTK;
using osuTK.Input;

namespace FlappyBird.Game;

public partial class FlappyBirdGame : FlappyBirdGameBase
{
    private readonly DrawSizePreservingFillContainer gameScreen 
        = new DrawSizePreservingFillContainer();

    private readonly TitleSprite gameOverSprite 
        = new TitleSprite(textureName: "gameover");
    private readonly TitleSprite logoSprite 
        = new TitleSprite(textureName: "message");
    private readonly ScreenFlash screenFlash 
        = new ScreenFlash();

    private readonly Bird bird 
        = new Bird();
    private readonly Obstacles obstacles 
        = new Obstacles();

    private Backdrop? skyBackdrop;
    private Backdrop? groundBackdrop;

    private readonly ScoreCounter scoreCounter 
        = new ScoreCounter();

    private DrawableSample? scoreSound;
    private DrawableSample? punchSound;
    private DrawableSample? fallSound;
    private DrawableSample? whooshSound;

    private GameState state = GameState.Ready;
    private bool disableInput;

    [BackgroundDependencyLoader]
    private void Load()
    {
        Add(scoreSound = new DrawableSample(sample: Audio.Samples.Get("point.ogg")));
        Add(punchSound = new DrawableSample(sample: Audio.Samples.Get("hit.ogg")));
        Add(fallSound = new DrawableSample(sample: Audio.Samples.Get("die.ogg")));
        Add(whooshSound = new DrawableSample(sample: Audio.Samples.Get("swoosh.ogg")));

        skyBackdrop = new Backdrop(createSprite: () => new BackdropSprite(), duration: 18000.0f);
        groundBackdrop = new Backdrop(createSprite: () => new GroundSprite(), duration: 1500.0f);

        gameScreen.Children = new Drawable[]
        {
            skyBackdrop,
            obstacles,
            bird,
            groundBackdrop,
            gameOverSprite,
            logoSprite,
            scoreCounter.ScoreSpriteText,
            screenFlash
        };

        gameScreen.Strategy = DrawSizePreservationStrategy.Minimum;
        gameScreen.TargetDrawSize = new Vector2(x: 0, y: 768);
        AddInternal(drawable: gameScreen);

        Action<int> value = _ =>
            {
                scoreCounter.IncrementScore();
                scoreSound.Play();
            };
        obstacles.thresholdCrossed = value;

        bird.groundY = 525.0;

        obstacles.birdThreshold = bird.X;
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        Ready();
    }

    protected override void Update()
    {
        base.Update();

        switch (state)
        {
            case GameState.Playing:
                if (obstacles.CheckForCollision(birdQuad: bird.CollisionQuad)
                    || bird.IsTouchingGround) 
                    ChangeGameState(newState: GameState.GameOver);
                break;
        }
    }

    protected override bool OnKeyDown(KeyDownEvent keyDownEvent)
    {
        if (keyDownEvent.Repeat) return base.OnKeyDown(e: keyDownEvent);

        if (keyDownEvent.Key == Key.Space && HandleTap()) return true;

        return base.OnKeyDown(e: keyDownEvent);
    }

    protected override bool OnMouseDown(MouseDownEvent mouseDownEvent)
    {
        switch (mouseDownEvent.MouseDownPosition.Y / DrawHeight)
        {
            case < 0.05f:
            case > 0.95f:
                return base.OnMouseDown(e: mouseDownEvent);
        }

        if (HandleTap()) return true;
        return base.OnMouseDown(e: mouseDownEvent);
    }

    private bool HandleTap()
    {
        if (!disableInput) switch (state)
            {
                case GameState.GameOver:
                    ChangeGameState(newState: GameState.Ready);
                    return true;
                case GameState.Playing:
                    bird.FlyUp();
                    return true;
                default:
                    ChangeGameState(newState: GameState.Playing);
                    return true;
            }
        return false;
    }

    private void ChangeGameState(GameState newState)
    {
        if (newState != state)
        {
            state = newState;

            switch (newState)
            {
                case GameState.Ready:
                    Ready();
                    break;
                case GameState.Playing:
                    Play();
                    break;
                case GameState.GameOver:
                    Fail();
                    break;
            }
        }
    }

    private void Ready()
    {
        Debug.Assert(condition: state == GameState.Ready);

        scoreCounter.Reset();

        SampleChannel? sampleChannel = whooshSound?.Play();

        screenFlash.Flash(fadeInDuration: 0.0, fadeOutDuration: 700.0);

        bird.Reset();
        obstacles.Reset();

        groundBackdrop?.Start();
        skyBackdrop?.Start();

        gameOverSprite.Hide();
        logoSprite.Show();
    }

    private void Play()
    {
        Debug.Assert(condition: state == GameState.Playing);

        obstacles.Start();
        logoSprite.Hide();
        scoreCounter.ScoreSpriteText.Show();

        bird.FlyUp();
    }

    private void Fail()
    {
        Debug.Assert(condition: state == GameState.GameOver);

        const double _fadeInDuration = 30.0;

        screenFlash.Flash(fadeInDuration: _fadeInDuration, fadeOutDuration: 500.0);
        Scheduler.AddDelayed(task: () => gameOverSprite.Show(), timeUntilRun: _fadeInDuration);

        punchSound?.Play();
        Scheduler.AddDelayed(task: () => fallSound?.Play(), timeUntilRun: 70.0);

        bird.FallDown();

        obstacles.Freeze();
        groundBackdrop?.Freeze();
        skyBackdrop?.Freeze();

        DisableGameInput(duration: 500.0f);
    }

    private void DisableGameInput(double duration)
    {
        disableInput = true;
        Scheduler.AddDelayed(task: () => disableInput = false, timeUntilRun: duration);
    }
}

public abstract partial class FlappyBirdGameBase : osu.Framework.Game
{
    protected override TextureFilteringMode DefaultTextureFilteringMode
        => TextureFilteringMode.Nearest;

    [BackgroundDependencyLoader]
    private void Load() 
        => Resources.AddStore(store: new DllResourceStore(assembly: FlappyBirdResources.ResourceAssembly));
}

