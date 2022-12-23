namespace FlappyBird.Game.Elements;

public partial class ScoreCounter
{
    private int _score;
    public int Score 
    { 
        get => _score; 
        set
        { 
            if(value != _score) _score = value; 
        }
    }

    public ScoreSpriteText ScoreSpriteText = new ScoreSpriteText();
    

    public void Reset()
    {
        Score = 0;
        ScoreSpriteText.Text = "0";
        ScoreSpriteText.Hide();

    }

    public void Start() 
        => ScoreSpriteText.Show();

    public void IncrementScore()
    {
        Score++;
        ScoreSpriteText.Text = Score.ToString();
    }

}