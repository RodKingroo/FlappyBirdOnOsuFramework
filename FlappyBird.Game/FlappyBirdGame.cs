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
using osuTK.Input;

namespace FlappyBird.Game
{
    public partial class FlappyBirdGame : FlappyBirdGameBase
    {
        private readonly DrawSizePreservingFillContainer gameScreen = new ();

        private readonly TitleSprite gameOverSprite = new ("gameover");
        private readonly TitleSprite logoSprite = new ("message");
        private readonly ScreenFlash screenFlash = new ();

        private readonly Bird bird = new ();
        private readonly Obstacles obstacles = new ();

        private Backdrop? skyBackdrop;
        private Backdrop? groundBackdrop;

        private readonly ScoreCounter scoreCounter = new ();

        private DrawableSample? scoreSound;
        private DrawableSample? punchSound;
        private DrawableSample? fallSound;
        private DrawableSample? whooshSound;

        private GameState state = GameState.Ready;
        private bool disableInput;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Add(scoreSound = new (Audio.Samples.Get("point.ogg")));
            Add(punchSound = new (Audio.Samples.Get("hit.ogg")));
            Add(fallSound = new (Audio.Samples.Get("die.ogg")));
            Add(whooshSound = new (Audio.Samples.Get("swoosh.ogg")));

            skyBackdrop = new (() => new BackdropSprite(), 20000.0f);
            groundBackdrop = new (() => new GroundSprite(), 2250.0f);

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
            gameScreen.TargetDrawSize = new (0, 768);
            AddInternal(gameScreen);

            obstacles.thresholdCrossed = _ =>
            {
                scoreCounter.IncrementScore();
                scoreSound.Play();
            };

            bird.groundY = 525.0f;

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
                    if (obstacles.CheckForCollision(bird.CollisionQuad) || bird.isTouchingGround) 
                        ChangeGameState(GameState.GameOver);
                    break;
            }
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Repeat) return base.OnKeyDown(e);

            if (e.Key == Key.Space && HandleTap()) return true;

            return base.OnKeyDown(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            float verticalOffset = e.MouseDownPosition.Y / DrawHeight;
            if (verticalOffset < 0.05f || verticalOffset > 0.95f)
                return base.OnMouseDown(e);

            if (HandleTap()) return true;

            return base.OnMouseDown(e);
        }

        private bool HandleTap()
        {
            if (disableInput) return false;

            switch (state)
            {
                case GameState.GameOver:
                    Reset();
                    return true; 
                case GameState.Playing:
                    bird.FlyUp();
                    return true; 
                default:
                    ChangeGameState(GameState.Playing);
                    return true;
            }
        }

        private void Reset() => ChangeGameState(GameState.Ready);

        private void ChangeGameState(GameState newState)
        {
            if (newState == state) return;

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

        private void Ready()
        {
            Debug.Assert(state == GameState.Ready);

            scoreCounter.Reset();

            whooshSound?.Play();

            screenFlash.Flash(0.0, 700.0);

            bird.Reset();
            obstacles.Reset();

            groundBackdrop?.Start();
            skyBackdrop?.Start();

            gameOverSprite.Hide();
            logoSprite.Show();
        }

        private void Play()
        {
            Debug.Assert(state == GameState.Playing);

            obstacles.Start();
            logoSprite.Hide();
            scoreCounter.ScoreSpriteText.Show();

            bird.FlyUp();
        }

        private void Fail()
        {
            Debug.Assert(state == GameState.GameOver);

            const double _fadeInDuration = 30.0;

            screenFlash.Flash(_fadeInDuration, 500.0);
            Scheduler.AddDelayed(() => gameOverSprite.Show(), _fadeInDuration);

            punchSound?.Play();
            Scheduler.AddDelayed(() => fallSound?.Play(), 70.0);

            bird.FallDown();

            obstacles.Freeze();
            groundBackdrop?.Freeze();
            skyBackdrop?.Freeze();

            DisableGameInput(500.0f);
        }

        private void DisableGameInput(double duration)
        {
            disableInput = true;
            Scheduler.AddDelayed(() => disableInput = false, duration);
        }
    }

    public abstract partial class FlappyBirdGameBase : osu.Framework.Game
    {
        protected override TextureFilteringMode DefaultTextureFilteringMode
            => TextureFilteringMode.Nearest;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Resources.AddStore(new DllResourceStore(FlappyBirdResources.ResourceAssembly));
        }
    }

}
