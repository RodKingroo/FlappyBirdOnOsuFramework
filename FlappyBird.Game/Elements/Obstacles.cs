using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Utils;
using osuTK;

namespace FlappyBird.Game.Elements;

public partial class Obstacles : CompositeDrawable
{
    private const double pipe_distance = 200.0;

    private bool _running;
    public bool Running 
    { 
        get => _running; 
        set
        {
            if(value != _running) _running = value;
        }  
    }

    public float birdThreshold = 0.0f;

    public Action<int>? thresholdCrossed;

    private int crossedThresholdCount;

    private Stack<Drawable> obstaclesToRemove = new Stack<Drawable>();

    private int obstacleCount;

    private bool frozen;
    

    private float pipeVelocity
    {
        get
        {
            return Clock.FramesPerSecond switch
            {
                > 0.0f => 185.0f * (float)(Clock.ElapsedFrameTime / 675.0f),
                _ => 0.0f
            };
        }
    }



    public Obstacles()
    {
        Anchor = Anchor.Centre;
        Origin = Anchor.Centre;
        RelativeSizeAxes = Axes.Both;
    }

    public void Start()
    {
        if (!Running) Running = true;
    }

    public void Freeze()
    {
        if (Running) frozen = true;
    }

    public void Reset()
    {
        if (Running)
        {
            Running = false;
            frozen = false;

            obstacleCount = 0;
            crossedThresholdCount = 0;

            ClearInternal();
        }
    }

    public bool CheckForCollision(Quad birdQuad) 
        => InternalChildren.Cast<PipeObstacle>().FirstOrDefault()?.CheckCollision(birdQuad: birdQuad) 
        ?? false;

    protected override void Update()
    {
        if (Running)
        {
            if (InternalChildren.Count == 0)
            {
                SpawnNewObstacle();
                return;
            }

            foreach (var drawable in InternalChildren)
            {
                if (frozen) break;

                PipeObstacle? obstacle = (PipeObstacle)drawable;
                obstacle.Position = new Vector2(x: obstacle.Position.X - pipeVelocity, y: 0.0f);

                if (obstacle.Position.X + obstacle.DrawWidth < 0.0f) 
                    obstaclesToRemove.Push(item: obstacle);

            }

            while (obstaclesToRemove.TryPop(result: out var obstacle))
            {
                RemoveInternal(drawable: obstacle, disposeImmediately: true);

                obstacleCount++;
            }

            var first = InternalChildren.First();

            if (first.X < birdThreshold && obstacleCount == crossedThresholdCount)
            {
                crossedThresholdCount++;

                thresholdCrossed?.Invoke(obj: crossedThresholdCount);
            }

            if (InternalChildren.Count > 0)
            {
                PipeObstacle lastObstacle = (PipeObstacle)InternalChildren.Last();
                if (lastObstacle.Position.X + lastObstacle.DrawWidth < DrawWidth - pipe_distance)
                    SpawnNewObstacle();
            }
            else SpawnNewObstacle();
        }
    }

    private void SpawnNewObstacle() 
        => AddInternal(drawable: new PipeObstacle
        {
            Position = new Vector2(x: DrawWidth, y: 0f),
            verticalPositionAdjust = RNG.NextSingle(minValue: -150.0f, maxValue: 100.0f)
        });
}