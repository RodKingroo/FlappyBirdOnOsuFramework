using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Utils;
using osuTK;

namespace FlappyBird.Game.Elements
{
    public class Obstacles : CompositeDrawable
    {
        public Vector2 CollisionBoxSize = new Vector2(50);

        public bool Running { get; set; }

        public float BirdThreshold = 0.0f;

        public Action<int> ThresholdCrossed;

        private int crossedThresholdCount;

        private Stack<Drawable> obstaclesToRemove = new Stack<Drawable>();

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

        private const float pipe_distance = 300.0f;

        public Obstacles()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;
        }

        public void Start()
        {
            if (Running) return;

            Running = true;
        }

        public void Freeze()
        {
            if (!Running) return;

            frozen = true;
        }

        public void Reset()
        {
            if (!Running) return;

            Running = false;
            frozen = false;

            obstacleCount = 0;
            crossedThresholdCount = 0;

            ClearInternal();
        }

        public bool CheckForCollision(Quad birdQuad) => InternalChildren.Cast<PipeObstacle>().FirstOrDefault()?.CheckCollision(birdQuad) ?? false;

        protected override void Update()
        {
            if (!Running) return;

            if (InternalChildren.Count == 0)
            {
                spawnNewObstacle();
                return;
            }

            foreach (var drawable in InternalChildren)
            {
                if (frozen) break;

                var obstacle = (PipeObstacle)drawable;
                obstacle.Position = new Vector2(obstacle.Position.X - pipeVelocity, 0.0f);

                if (obstacle.Position.X + obstacle.DrawWidth < 0.0f) obstaclesToRemove.Push(obstacle);
                
            }

            while (obstaclesToRemove.TryPop(out var obstacle))
            {
                RemoveInternal(obstacle, true);

                obstacleCount++;
            }

            var first = InternalChildren.First();

            if (first.X < BirdThreshold && obstacleCount == crossedThresholdCount)
            {
                crossedThresholdCount++;

                ThresholdCrossed?.Invoke(crossedThresholdCount);
            }


            if (InternalChildren.Count > 0)
            {
                var lastObstacle = (PipeObstacle)InternalChildren.Last();
                if (lastObstacle.Position.X + lastObstacle.DrawWidth < DrawWidth - pipe_distance) 
                    spawnNewObstacle();
            }
            else spawnNewObstacle();
        }

        private void spawnNewObstacle()
        {
            AddInternal(new PipeObstacle
            {
                Position = new Vector2(DrawWidth, 10.0f),
                VerticalPositionAdjust = RNG.NextSingle(-150.0f, 50.0f)
            });
        }
    }
}