public class GameplayService
{
    private GameplayController gameplayController;
    public GameplayService(GameplaySO gameplayso,CardSO cardso) => gameplayController = new GameplayController(gameplayso.GameplayView,cardso);
    public void Play(Difficulty difficulty) => gameplayController.Play(difficulty);
    public int GetAttemptsCount() => gameplayController.GetAttemptsCount();
    public float GetTime() => gameplayController.GetTime();
    public void SetTime(float time) => gameplayController.SetTime(time);
    public void ToggleGameplayCanvas(bool toggle) => gameplayController.ToggleGameplayCanvas(toggle);
    public void RestartGame() => gameplayController.RestartGame(Difficulty.Normal);
}