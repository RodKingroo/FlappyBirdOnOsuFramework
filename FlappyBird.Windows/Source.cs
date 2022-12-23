using FlappyBird.Game;
using osu.Framework;
using osu.Framework.Platform;

namespace FlappyBird.Windows;

public class Source
{
    private const string _baseGameName = @"Flappy Bird";

    [System.STAThread]
    public static void Main(string[] args)
    {
        string gameName = _baseGameName;

        GameHost host = Host.GetSuitableDesktopHost(gameName);
        osu.Framework.Game game = new FlappyBirdGame();
        host.Run(game: game);
    }
}
