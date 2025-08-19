public class GameplayService
{
    private GameplayController gameplayController;
    public GameplayService(GameplaySO gameplayso,CardSO cardso)
    {
        gameplayController = new GameplayController(gameplayso.GameplayView,cardso,Difficulty.Hard);
    }
}
