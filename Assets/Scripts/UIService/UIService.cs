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

    [Header("Gameplay")]
    [SerializeField] private RectTransform Gameplay;
    [SerializeField] private Button RestartButton;
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

    private void Awake()
    {
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
        Restart.onClick.AddListener(OnRestartButtonClicked);
        Back.onClick.AddListener(OnBackButtonClicked);
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
        GameService.Instance.GameplayService.Play(Difficulty.Normal);
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
        if (Gameover.gameObject.activeInHierarchy)
        {
            Gameover.gameObject.SetActive(false);
            Gameplay.gameObject.SetActive(false);

            GameService.Instance.GameplayService.ToggleGameplayCanvas(false);
        }
        else
        {
            Info.gameObject.SetActive(false);
        }
        MainMenu.gameObject.SetActive(true);
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
        if(choice)
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
        int score = CalculateScore(attempts,time);
        finalAttempts.text = attempts.ToString();
        finalTime.text = FormatTime(time);
        finalScore.text = score.ToString();

        Gameover.gameObject.SetActive(true);
    }
    private int CalculateScore(int attempts,float time)
    {
        int baseScore = 1000;
        int attemptsPenalty = attempts * 15;
        int timePenalty = Mathf.FloorToInt(time * 2f);

        return Mathf.Max(0, baseScore - attemptsPenalty - timePenalty);
    }
}
