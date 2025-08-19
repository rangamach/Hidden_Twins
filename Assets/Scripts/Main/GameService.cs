using UnityEngine;

public class GameService : GenericMonoSingleton<GameService>
{
    //Services:
    public GameplayService GameplayService { get; private set; }

    [Header("UI")]
    [SerializeField] private UIService uiService;
    public UIService UIService => uiService;

    [Header("Gameplay")]
    [SerializeField] private GameplaySO gameplaySO;
    [SerializeField] private CardSO cardSO;
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
