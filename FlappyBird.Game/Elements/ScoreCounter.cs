namespace FlappyBird.Game.Elements
{
    public partial class ScoreCounter
    {
        public int score { get; set; }

        public ScoreSpriteText ScoreSpriteText = new();

        public void Reset()
        {
            score = 0;
            ScoreSpriteText.Text = "0";
            ScoreSpriteText.Hide();

        }

        public void Start() => ScoreSpriteText.Show();

        public void IncrementScore()
        {
            score++;
            ScoreSpriteText.Text = score.ToString();
        }

    }
}