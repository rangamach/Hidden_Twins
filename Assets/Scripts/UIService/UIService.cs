using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIService : MonoBehaviour
{
    [Header("Start")]
    [SerializeField] private RectTransform MainMenu;
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button InfoButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private RectTransform Info;
    [SerializeField] private Button BackButton;
    [SerializeField] private TMP_Dropdown DifficultyDropdown;
    [SerializeField] private TextMeshProUGUI BestStatsStartText;
    [SerializeField] private TextMeshProUGUI BestTimeStartText;
    [SerializeField] private TextMeshProUGUI BestAttemptsStartText;
    [SerializeField] private TextMeshProUGUI BestScoreStartText;

    [Header("Gameplay")]
    [SerializeField] private RectTransform Gameplay;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private TextMeshProUGUI attemptsCountText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Image Tick;
    [SerializeField] private Image X;

    [Header("Gameover")]
    [SerializeField] private Button Restart;
    [SerializeField] private Button Back;
    [SerializeField] private RectTransform Gameover;
    [SerializeField] private TextMeshProUGUI finalAttempts;
    [SerializeField] private TextMeshProUGUI finalTime;
    [SerializeField] private TextMeshProUGUI finalScore;
    [SerializeField] private TextMeshProUGUI bestStatsText;
    [SerializeField] private TextMeshProUGUI bestTime;
    [SerializeField] private TextMeshProUGUI bestAttempt;
    [SerializeField] private TextMeshProUGUI bestScore;
    [SerializeField] private Image bestTimeHL;
    [SerializeField] private Image bestAttemptHL;
    [SerializeField] private Image bestScoreHL;

    private void Awake()
    {
        InitializeKeys();

        AddButtonOnClicks();

        MainMenu.gameObject.SetActive(true);
    }
    //Adds listeners to all buttons
    private void AddButtonOnClicks()
    {
        PlayButton.onClick.AddListener(OnPlayButtonClicked);
        InfoButton.onClick.AddListener(OnInfoButtonClicked);
        BackButton.onClick.AddListener(OnBackButtonClicked);
        ExitButton.onClick.AddListener(OnExitButtonClicked);
        RestartButton.onClick.AddListener(OnRestartButtonClicked);
        QuitButton.onClick.AddListener(OnBackButtonClicked);
        Restart.onClick.AddListener(OnRestartButtonClicked);
        Back.onClick.AddListener(OnBackButtonClicked);

        DifficultyDropdown.onValueChanged.AddListener(OnDifficultyChanged);
    }
    private void InitializeKeys()
    {
        const string key = "Initialized";

        if (PlayerPrefs.GetInt(key, 0) == 0)
        {
            foreach (Difficulty difficulty in System.Enum.GetValues(typeof(Difficulty)))
            {
                string timeKey = GetTimeKey(difficulty);
                string attemptsKey = GetAttemptsKey(difficulty);
                string scoreKey = GetScoreKey(difficulty);

                PlayerPrefs.SetFloat(timeKey, float.MaxValue);
                PlayerPrefs.SetInt(attemptsKey, int.MaxValue);
                PlayerPrefs.SetInt(scoreKey, 0);
            }

            PlayerPrefs.SetInt(key, 1);
            PlayerPrefs.Save();
        }
    }
    private void Start()
    {
        ShowBestStats(GetDifficulty());
    }
    private void Update()
    {
        //update only if in gameplay mode
        if (Gameplay.gameObject.activeInHierarchy)
        {
            UpdateTimerText();
            UpdateAttemptsCountText();
        }
    }
    //Start:
    private void OnPlayButtonClicked()
    {
        GameService.Instance.SoundService.PlaySFX(SoundType.Game_Start);
        GameService.Instance.GameplayService.Play(GetDifficulty());
        MainMenu.gameObject.SetActive(false);
        Gameplay.gameObject.SetActive(true);

    }
    private void OnInfoButtonClicked()
    {
        GameService.Instance.SoundService.PlaySFX(SoundType.Button_Click);
        MainMenu.gameObject.SetActive(false);
        Info.gameObject.SetActive(true);
    }
    private void OnBackButtonClicked()
    {
        GameService.Instance.SoundService.PlaySFX(SoundType.Button_Click);
        if (Gameover.gameObject.activeInHierarchy || Gameplay.gameObject.activeInHierarchy)
        {
            Gameover.gameObject.SetActive(false);
            Gameplay.gameObject.SetActive(false);

            GameService.Instance.GameplayService.ToggleGameplayCanvas(false);
        }
        else if (Info.gameObject.activeInHierarchy)
        {
            Info.gameObject.SetActive(false);
        }

        ShowBestStats(GetDifficulty()); 

        MainMenu.gameObject.SetActive(true);
    }
    private Difficulty GetDifficulty()
    {
        switch (DifficultyDropdown.value)
        {
            case 0:
                return Difficulty.Easy;
            case 1:
                return Difficulty.Normal;
            case 2:
                return Difficulty.Hard;
            case 3:
                return Difficulty.VeryHard;
            default:
                return Difficulty.Normal;
        }
    }
    private void OnDifficultyChanged(int value)
    {
        GameService.Instance.SoundService.PlaySFX(SoundType.Button_Click);
        ShowBestStats(GetDifficulty());
    }
    private void ShowBestStats(Difficulty selectedDifficulty)
    {
        BestStatsStartText.text = $"{selectedDifficulty} Best Stats";

        string timeKey = GetTimeKey(selectedDifficulty);
        string attemptsKey = GetAttemptsKey(selectedDifficulty);
        string scoreKey = GetScoreKey(selectedDifficulty);

        float bestTime = PlayerPrefs.GetFloat(timeKey);
        float bestAttempt = PlayerPrefs.GetInt(attemptsKey);
        float bestScore = PlayerPrefs.GetInt(scoreKey);

        BestTimeStartText.text = bestTime == float.MaxValue ? "--:--:--" : FormatTime(bestTime);
        BestAttemptsStartText.text = bestAttempt == int.MaxValue ? "--" : bestAttempt.ToString();
        BestScoreStartText.text = bestScore.ToString();
    }
    private void OnExitButtonClicked()
    {
        GameService.Instance.SoundService.PlaySFX(SoundType.Button_Click);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    //Gameplay:
    private void OnRestartButtonClicked()
    {
        GameService.Instance.SoundService.PlaySFX(SoundType.Game_Start);
        if (Gameover.gameObject.activeInHierarchy)
        {
            Gameover.gameObject.SetActive(false);
            Gameplay.gameObject.SetActive(true);
        }

        GameService.Instance.GameplayService.RestartGame();

    }
    private void UpdateAttemptsCountText()
    {
        attemptsCountText.text = GameService.Instance.GameplayService.GetAttemptsCount().ToString();
    }
    private void UpdateTimerText()
    {
        float currentTime = GameService.Instance.GameplayService.GetTime();
        currentTime += Time.deltaTime;

        timerText.text = FormatTime(currentTime);

        GameService.Instance.GameplayService.SetTime(currentTime);
    }
    private string FormatTime(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt(time % 3600 / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        if (hours > 0)
        {
            return $"{hours:00}:{minutes:00}:{seconds:00}";
        }
        else
        {
            return $"{minutes:00}:{seconds:00}";
        }
    }
    public void ShowTickOrX(bool choice)
    {
        Image img;
        if (choice)
        {
            img = Tick;
        }
        else
        {
            img = X;
        }
        StartCoroutine(TickXCouroutine(img));
    }
    private IEnumerator TickXCouroutine(Image img)
    {
        img.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        img.gameObject.SetActive(false);
    }

    //Game Over
    public void GameOver()
    {
        Gameplay.gameObject.SetActive(false);

        int attempts = GameService.Instance.GameplayService.GetAttemptsCount();
        float time = GameService.Instance.GameplayService.GetTime();
        int score = CalculateScore(attempts, time);

        finalAttempts.text = attempts.ToString();
        finalTime.text = FormatTime(time);
        finalScore.text = score.ToString();

        CheckAndUpdateBestStats(time, attempts, score);

        Gameover.gameObject.SetActive(true);
    }
    private int CalculateScore(int attempts, float time)
    {
        int baseScore = 10000;
        int attemptsPenalty = attempts * 5;
        int timePenalty = Mathf.FloorToInt(time * 2f);

        return Mathf.Max(0, baseScore - attemptsPenalty - timePenalty);
    }
    private void CheckAndUpdateBestStats(float time, int attempts, int score)
    {
        Difficulty currentDifficulty = GameService.Instance.GameplayService.GetCurrentDifficulty();

        bestStatsText.text = $"{currentDifficulty} Best Stats";

        bestTimeHL.gameObject.SetActive(false);
        bestAttemptHL.gameObject.SetActive(false);
        bestScoreHL.gameObject.SetActive(false);

        string timeKey = GetTimeKey(currentDifficulty);
        string attemptsKey = GetAttemptsKey(currentDifficulty);
        string scoreKey = GetScoreKey(currentDifficulty);

        float bestTimeSoFar = PlayerPrefs.GetFloat(timeKey);
        int bestAttemptSoFar = PlayerPrefs.GetInt(attemptsKey);
        int bestScoreSoFar = PlayerPrefs.GetInt(scoreKey);

        bool isUpdated = false;

        if (time < bestTimeSoFar)
        {
            PlayerPrefs.SetFloat(timeKey, time);
            bestTimeHL.gameObject.SetActive(true);
            isUpdated = true;
        }
        if (attempts < bestAttemptSoFar)
        {
            PlayerPrefs.SetInt(attemptsKey, attempts);
            bestAttemptHL.gameObject.SetActive(true);
            isUpdated = true;
        }
        if (score > bestScoreSoFar)
        {
            PlayerPrefs.SetInt(scoreKey, score);
            bestScoreHL.gameObject.SetActive(true);
            isUpdated = true;
        }

        bestTime.text = FormatTime(PlayerPrefs.GetFloat(timeKey));
        bestAttempt.text = PlayerPrefs.GetInt(attemptsKey).ToString();
        bestScore.text = PlayerPrefs.GetInt(scoreKey).ToString();

        if (isUpdated)
        {
            PlayerPrefs.Save();
        }
    }
    private string GetTimeKey(Difficulty difficulty) => $"BestTime_{difficulty}";
    private string GetAttemptsKey(Difficulty difficulty) => $"BestAttempts_{difficulty}";
    private string GetScoreKey(Difficulty difficulty) => $"BestScore_{difficulty}";
}
