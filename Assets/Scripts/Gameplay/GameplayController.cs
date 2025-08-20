using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController
{
    private int maxCards = 16;
    public GameplayView gameplayView { get; private set; }
    public GameplayModel gameplayModel { get; private set; }
    public CardSO CardSO { get; private set; }

    private List<CardController> activeCards = new List<CardController>();
    private List<CardController> cardControllers = new List<CardController>();   
    private System.Random randomNumberGenerator = new System.Random();

    private CardController firstFlippedCard = null;
    private CardController secondFlippedCard = null;
    private bool isCheckingMatch = false;
    public GameplayController(GameplayView gameplayView,CardSO cardSO)
        {
            this.gameplayView = Object.Instantiate(gameplayView.gameObject).GetComponent<GameplayView>();
            this.gameplayModel = new GameplayModel();

            this.CardSO = cardSO;

            this.gameplayView.SetController(this);

            for (int i = 0; i < maxCards;i++)
            {
                var controller = new CardController(CardSO.cardPrefab, cardSO.BackImage);
                controller.CardView.gameObject.SetActive(false);
                cardControllers.Add(controller);
                this.gameplayView.PlaceCards(controller.CardView, 6);
            }
        }
    private void CreateBoard(Difficulty difficulty)
    {
        gameplayModel.SetDifficulty(difficulty);
        gameplayModel.SetTotalAttempts(0);
        gameplayModel.SetTime(0);
        int gridSize = gameplayModel.gridSize;
        int totalCards = gridSize * gridSize;

        List<Sprite> chosenFronts = new List<Sprite>();
        ShuffleDeck(CardSO.FrontImages);

        for (int i = 0; i < totalCards / 2; i++)
        {
            chosenFronts.Add(CardSO.FrontImages[i]);
            chosenFronts.Add(CardSO.FrontImages[i]);
        }
        ShuffleDeck(chosenFronts);

        for (int i = 0; i < maxCards; i++)
        {
            if (i < totalCards)
            {
                cardControllers[i].ResetCard(chosenFronts[i]);
                cardControllers[i].CardView.SetCardSize(GetCardSize(difficulty));
                cardControllers[i].CardView.gameObject.SetActive(true);
                cardControllers[i].SetGameplayController(this);
            }
            else
            {
                cardControllers[i].CardView.gameObject.SetActive(false);
            }
        }
        gameplayView.AdjustGrid(gridSize);

        ToggleGameplayCanvas(true);
    }
    private Vector2 GetCardSize(Difficulty difficulty)
    {
        switch(difficulty)
        {
            case Difficulty.Normal:
                return new Vector2(1.75f, 1.75f);
            default:
                return new Vector2(1.75f, 1.75f);
        }
    }
    private void ShuffleDeck<T>(List<T> list)
    {
        int size = list.Count;
        while(size > 1)
        {
            size--;
            int random = randomNumberGenerator.Next(size + 1);
            (list[random], list[size]) = (list[size], list[random]);
        }
    }
    public void RestartGame(Difficulty difficulty)
    {
        CreateBoard(difficulty);
    }
    public void OnCardFlipped(CardController card)
    {
        if (isCheckingMatch) return;

        if (firstFlippedCard == null)
        {
            firstFlippedCard = card;
        }
        else if (secondFlippedCard == null && card != firstFlippedCard)
        {
            secondFlippedCard = card;
            GameService.Instance.StartCoroutine(CheckMatchRoutine());
        }
    }
    private IEnumerator CheckMatchRoutine()
    {
        isCheckingMatch = true;

        yield return new WaitForSeconds(0.5f);

        if (firstFlippedCard.CardModel.FrontImage == secondFlippedCard.CardModel.FrontImage)
        {
            firstFlippedCard.MarkMatched();
            secondFlippedCard.MarkMatched();

            if(WinCheck())
            {
                GameService.Instance.UIService.ShowTickOrX(true);
                OnGameWon();
            }
            else
            {
                GameService.Instance.UIService.ShowTickOrX(true);
                GameService.Instance.SoundService.PlaySFX(SoundType.Match);
            }
        }
        else
        {
            GameService.Instance.UIService.ShowTickOrX(false);
            GameService.Instance.SoundService.PlaySFX(SoundType.No_Match);
            firstFlippedCard.HideCard();
            secondFlippedCard.HideCard();
        }

        gameplayModel.SetTotalAttempts(GetAttemptsCount() + 1);

        firstFlippedCard = null;
        secondFlippedCard = null;
        isCheckingMatch = false;
    }
    public bool IsBusyChecking() => isCheckingMatch;
    private bool WinCheck()
    {
        foreach (var controller in cardControllers)
        {
            if(controller.CardView.gameObject.activeSelf && !controller.CardModel.IsMatched)
            {
                return false;
            }
        }
        GameService.Instance.SoundService.PlaySFX(SoundType.Match);
        return true;
    }
    private void OnGameWon()
    {
        GameService.Instance.SoundService.PlaySFX(SoundType.Won);
        GameService.Instance.UIService.GameOver(); 
    }
    public void Play(Difficulty difficulty) => CreateBoard(difficulty);
    public int GetAttemptsCount() => gameplayModel.TotalAttempts;
    public float GetTime() => gameplayModel.Time;
    public void SetTime(float time) => gameplayModel.SetTime(time);
    public void ToggleGameplayCanvas(bool toggle) => gameplayView.gameObject.SetActive(toggle);
}


