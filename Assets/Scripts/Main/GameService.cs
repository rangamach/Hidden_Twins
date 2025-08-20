using UnityEngine;

public class GameService : GenericMonoSingleton<GameService>
{
    //Services:
    public GameplayService GameplayService { get; private set; }
    public SoundService SoundService { get; private set; }

    [Header("UI")]
    [SerializeField] private UIService uiService;
    public UIService UIService => uiService;

    [Header("Gameplay")]
    [SerializeField] private GameplaySO gameplaySO;
    [SerializeField] private CardSO cardSO;

    [Header("Sound")]
    [SerializeField] private SoundSO soundSO;
    [SerializeField] private AudioSource bgAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    private void Awake()
    {
        base.Awake();

        CreateServices();
    }
    private void CreateServices()
    {
        GameplayService = new GameplayService(gameplaySO,cardSO);
        SoundService = new SoundService(soundSO,bgAudioSource,sfxAudioSource);
    }
}
