using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Utils;
using osuTK;

namespace FlappyBird.Game.Elements
{
    public partial class Obstacles : CompositeDrawable
    {
        public Vector2 collisionBoxSize = new(50);

        public bool running { get; set; }

        public float birdThreshold = 0.0f;

        public Action<int> thresholdCrossed;

        private int crossedThresholdCount;

        private Stack<Drawable> obstaclesToRemove = new();

        private int obstacleCount;

        private bool frozen;

        private float pipeVelocity
        {
            get
            {
                if (Clock.FramesPerSecond > 0.0f) return 185.0f * (float)(Clock.ElapsedFrameTime / 675.0f);

                return 0.0f;
            }
        }

        private const float pipe_distance = 350.0f;

        public Obstacles()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;
        }

        public void Start()
        {
            if (running) return; 
            running = true;

        }

        public void Freeze()
        {
            if (!running) return;
            frozen = true;
        }

        public void Reset()
        {
            if (!running) return;

            running = false;
            frozen = false;

            obstacleCount = 0;
            crossedThresholdCount = 0;

            ClearInternal();
        }

        public bool CheckForCollision(Quad birdQuad) => InternalChildren.Cast<PipeObstacle>().FirstOrDefault()?.CheckCollision(birdQuad) ?? false;

        protected override void Update()
        {
            if (!running) return;

            if (InternalChildren.Count == 0)
            {
                SpawnNewObstacle();
                return;
            }

            foreach (var drawable in InternalChildren)
            {
                if (frozen) break;

                PipeObstacle? obstacle = (PipeObstacle)drawable;
                obstacle.Position = new Vector2(obstacle.Position.X - pipeVelocity, 0.0f);

                if (obstacle.Position.X + obstacle.DrawWidth < 0.0f) obstaclesToRemove.Push(obstacle);
                
            }

            while (obstaclesToRemove.TryPop(out var obstacle))
            {
                RemoveInternal(obstacle, true);

                obstacleCount++;
            }

            var first = InternalChildren.First();

            if (first.X < birdThreshold && obstacleCount == crossedThresholdCount)
            {
                crossedThresholdCount++;

                thresholdCrossed?.Invoke(crossedThresholdCount);
            }

            if (InternalChildren.Count > 0)
            {
                PipeObstacle lastObstacle = (PipeObstacle)InternalChildren.Last();
                if (lastObstacle.Position.X + lastObstacle.DrawWidth < DrawWidth - pipe_distance) 
                    SpawnNewObstacle();
            }
            else SpawnNewObstacle();
        }

        private void SpawnNewObstacle()
        {
            AddInternal(new PipeObstacle
            {
                Position = new(DrawWidth, 10.0f),
                VerticalPositionAdjust = RNG.NextSingle(-150.0f, 50.0f)
            });
        }
    }
}