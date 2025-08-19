using UnityEngine.UI;

public enum Difficulty
{
    Easy,
    Normal,
}

public class GameplayModel
{
    public Difficulty CurrentDifficulty { get; private set; }
    public int gridSize { get; private set; }

    public GameplayModel(Difficulty difficulty)
    {
        SetDifficulty(difficulty);
    }
    public void SetDifficulty(Difficulty difficulty)
    {
        this.CurrentDifficulty = difficulty;

        SetGridSize(difficulty);
    }
    private void SetGridSize(Difficulty difficulty)
    {
        switch(difficulty)
        {
            case Difficulty.Easy:
                this.gridSize = 2;
                break;
            case Difficulty.Normal:
                this.gridSize = 4;
                break;
        }
    }
}
