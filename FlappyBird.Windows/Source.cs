using FlappyBird.Game;
using osu.Framework;
using osu.Framework.Platform;

namespace FlappyBird.Windows
{
    public class Source
    {
        public static void Main()
        {
            using(GameHost host = Host.GetSuitableDesktopHost(@"Flappy Bird"))
            using(osu.Framework.Game game = new FlappyBirdGame())
            host.Run(game);
        }
    }
}
