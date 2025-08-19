using System.Collections.Generic;
using UnityEngine;

public class GameplayController
{
    private int maxCards = 36;
    public GameplayView gameplayView { get; private set; }
    public GameplayModel gameplayModel { get; private set; }
    public CardSO CardSO { get; private set; }

    private List<CardController> activeCards = new List<CardController>();
    private List<CardController> cardControllers = new List<CardController>();   
    private System.Random randomNumberGenerator = new System.Random();
        public GameplayController(GameplayView gameplayView,CardSO cardSO, Difficulty difficulty)
        {
            this.gameplayView = Object.Instantiate(gameplayView.gameObject).GetComponent<GameplayView>();
            this.gameplayModel = new GameplayModel(difficulty);

            this.CardSO = cardSO;

            this.gameplayView.SetController(this);

            for (int i = 0; i < maxCards;i++)
            {
                var controller = new CardController(CardSO.cardPrefab, cardSO.BackImage);
                controller.CardView.gameObject.SetActive(false);
                cardControllers.Add(controller);
                this.gameplayView.PlaceCards(controller.CardView, 6);
            }
            CreateBoard(difficulty);
        }
    private void CreateBoard(Difficulty difficulty)
    {
        gameplayModel.SetDifficulty(difficulty);
        int gridSize = gameplayModel.gridSize;
        int totalCards = gridSize * gridSize;

        List<Sprite> chosenFronts = new List<Sprite>();
        ShuffleDeck(CardSO.FrontImages);

        for (int i = 0; i < totalCards/2; i++)
        {
            chosenFronts.Add(CardSO.FrontImages[i]);
            chosenFronts.Add(CardSO.FrontImages[i]);
        }
        ShuffleDeck(chosenFronts);

        for(int i = 0;i<maxCards; i++)
        {
            if(i<totalCards)
            {
                cardControllers[i].ResetCard(chosenFronts[i]);
                cardControllers[i].CardView.gameObject.SetActive(true);
            }
            else
            {
                cardControllers[i].CardView.gameObject.SetActive(false);
            }
        }
        gameplayView.AdjustGrid(gridSize);
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
        //gameplayModel.SetDifficulty(difficulty);
        CreateBoard(difficulty);
    }
}
