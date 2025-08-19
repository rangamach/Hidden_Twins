using UnityEngine;

public class GameService : GenericMonoSingleton<GameService>
{
    //Services:
    public GameplayService GameplayService { get; private set; }

    [Header("Gameplay")]
    [SerializeField] GameplaySO gameplaySO;
    [SerializeField] CardSO cardSO;
    private void Awake()
    {
        base.Awake();

        CreateServices();
    }
    private void CreateServices()
    {
        GameplayService = new GameplayService(gameplaySO,cardSO);
    }
}
