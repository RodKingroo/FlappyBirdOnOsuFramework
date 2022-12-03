namespace FlappyBird.Game.Elements
{
    public class ScoreCounter
    {
        public int Score { get; set; }

        public ScoreSpriteText ScoreSpriteText = new();

        public void Reset()
        {
            Score = 0;
            ScoreSpriteText.Text = "0";
            ScoreSpriteText.Hide();

        }

        public void Start() => ScoreSpriteText.Show();

        public void IncrementScore()
        {
            Score++;
            ScoreSpriteText.Text = Score.ToString();
        }

    }
}