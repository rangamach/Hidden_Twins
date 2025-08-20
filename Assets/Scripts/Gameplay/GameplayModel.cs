public enum Difficulty
{
    Normal,
}

public class GameplayModel
{
    public Difficulty CurrentDifficulty { get; private set; }
    public int TotalAttempts { get; private set; }
    public float Time { get; private set; }
    public int gridSize { get; private set; }
    public void SetDifficulty(Difficulty difficulty)
    {
        this.CurrentDifficulty = difficulty;

        SetGridSize(difficulty);
    }
    //Sets grid size based on difficulty
    private void SetGridSize(Difficulty difficulty)
    {
        switch(difficulty)
        {
            case Difficulty.Normal:
                this.gridSize = 4;
                break;
        }
    }
    public void SetTotalAttempts(int attempts) => this.TotalAttempts = attempts;
    public void SetTime(float time) => this.Time = time;
}
