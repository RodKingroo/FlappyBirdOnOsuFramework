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
using osuTK;
using osuTK.Input;

namespace FlappyBird.Game
{
    public partial class FlappyBirdGame : FlappyBirdGameBase
    {
        private readonly DrawSizePreservingFillContainer gameScreen = new DrawSizePreservingFillContainer();

        private readonly TitleSprite gameOverSprite = new TitleSprite("gameover");
        private readonly TitleSprite logoSprite = new TitleSprite("message");
        private readonly ScreenFlash screenFlash = new ScreenFlash();

        private readonly Bird bird = new Bird();
        private readonly Obstacles obstacles = new Obstacles();

        private Backdrop skyBackdrop;
        private Backdrop groundBackdrop;

        private readonly ScoreCounter scoreCounter = new ScoreCounter();

        private DrawableSample scoreSound;
        private DrawableSample punchSound;
        private DrawableSample fallSound;
        private DrawableSample whooshSound;

        private GameState state = GameState.Ready;
        private bool disableInput;

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(scoreSound = new DrawableSample(Audio.Samples.Get("point.ogg")));
            Add(punchSound = new DrawableSample(Audio.Samples.Get("hit.ogg")));
            Add(fallSound = new DrawableSample(Audio.Samples.Get("die.ogg")));
            Add(whooshSound = new DrawableSample(Audio.Samples.Get("swoosh.ogg")));

            skyBackdrop = new Backdrop(() => new BackdropSprite(), 20000.0f);
            groundBackdrop = new Backdrop(() => new GroundSprite(), 2250.0f);

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
            gameScreen.TargetDrawSize = new Vector2(0, 768);
            AddInternal(gameScreen);

            obstacles.ThresholdCrossed = _ =>
            {
                scoreCounter.IncrementScore();
                scoreSound.Play();
            };

            bird.GroundY = 525.0f;

            obstacles.BirdThreshold = bird.X;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            ready();
        }

        protected override void Update()
        {
            base.Update();

            switch (state)
            {
                case GameState.Playing:
                    if (obstacles.CheckForCollision(bird.CollisionQuad) || bird.IsTouchingGround)
                        changeGameState(GameState.GameOver);
                    break;
            }
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Repeat) return base.OnKeyDown(e);

            if (e.Key == Key.Space && handleTap()) return true;

            return base.OnKeyDown(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            float verticalOffset = e.MouseDownPosition.Y / DrawHeight;
            if (verticalOffset < 0.05f || verticalOffset > 0.95f)
                return base.OnMouseDown(e);

            if (handleTap()) return true;

            return base.OnMouseDown(e);
        }

        private bool handleTap()
        {
            if (disableInput) return false;

            switch (state)
            {
                case GameState.GameOver:
                    reset();
                    return true;

                case GameState.Playing:
                    bird.FlyUp();
                    return true;

                default:
                    changeGameState(GameState.Playing);
                    return true;
            }
        }

        private void reset() => changeGameState(GameState.Ready);

        private void changeGameState(GameState newState)
        {
            if (newState == state) return;

            state = newState;

            switch (newState)
            {
                case GameState.Ready:
                    ready();
                    break;

                case GameState.Playing:
                    play();
                    break;

                case GameState.GameOver:
                    fail();
                    break;
            }
        }

        private void ready()
        {
            Debug.Assert(state == GameState.Ready);

            scoreCounter.Reset();

            whooshSound.Play();

            screenFlash.Flash(0.0, 700.0);

            bird.Reset();
            obstacles.Reset();

            groundBackdrop.Start();
            skyBackdrop.Start();

            gameOverSprite.Hide();
            logoSprite.Show();
        }

        private void play()
        {
            Debug.Assert(state == GameState.Playing);

            obstacles.Start();
            logoSprite.Hide();
            scoreCounter.ScoreSpriteText.Show();

            bird.FlyUp();
        }

        private void fail()
        {
            Debug.Assert(state == GameState.GameOver);

            const double fade_in_duration = 30.0;

            screenFlash.Flash(fade_in_duration, 500.0);
            Scheduler.AddDelayed(() => gameOverSprite.Show(), fade_in_duration);

            punchSound.Play();
            Scheduler.AddDelayed(() => fallSound.Play(), 70.0);

            bird.FallDown();

            obstacles.Freeze();
            groundBackdrop.Freeze();
            skyBackdrop.Freeze();

            disableGameInput(500.0f);
        }

        private void disableGameInput(double duration)
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
        private void load()
        {
            Resources.AddStore(new DllResourceStore(FlappyBirdResources.ResourceAssembly));
        }
    }

}
