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

    private void Awake()
    {
        PlayButton.onClick.AddListener(OnPlayButtonClicked);
        InfoButton.onClick.AddListener(OnInfoButtonClicked);
        BackButton.onClick.AddListener(OnBackButtonClicked);
        ExitButton.onClick.AddListener(OnExitButtonClicked);
        RestartButton.onClick.AddListener(OnRestartButtonClicked);

        MainMenu.gameObject.SetActive(true);
    }

    //Start:
    private void OnPlayButtonClicked()
    {
        GameService.Instance.GameplayService.Play(Difficulty.Normal);
        MainMenu.gameObject.SetActive(false);
        Gameplay.gameObject.SetActive(true);

    }
    private void OnInfoButtonClicked()
    {
        MainMenu.gameObject.SetActive(false);
        Info.gameObject.SetActive(true);
    }
    private void OnBackButtonClicked()
    {
        Info.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(true);
    }
    private void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    //Gameplay:
    private void OnRestartButtonClicked()
    {
        GameService.Instance.GameplayService.RestartGame();
    }
}
