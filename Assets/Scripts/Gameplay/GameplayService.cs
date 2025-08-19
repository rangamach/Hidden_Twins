public class GameplayService
{
    private GameplayController gameplayController;
    public GameplayService(GameplaySO gameplayso,CardSO cardso)
    {
        gameplayController = new GameplayController(gameplayso.GameplayView,cardso);  
    }
    public void Play(Difficulty difficulty) => gameplayController.Play(difficulty);
    public void RestartGame() => gameplayController.RestartGame(Difficulty.Normal);
}