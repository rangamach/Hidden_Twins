using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class CardController
{
    public CardView CardView { get; private set; }
    public CardModel CardModel { get; private set; }
    private GameplayController gameplayController;
    public CardController(CardView prefab, Sprite back)
    {
        this.CardView = Object.Instantiate(prefab.gameObject).GetComponent<CardView>();
        this.CardModel = new CardModel(back);

        this.CardView.SetBackSprite(this.CardModel.BackImage);

        CardView.SetController(this);
    }
    public void SetGameplayController(GameplayController controller) => this.gameplayController = controller;
    public void OnCardClicked()
    {
        if (CardModel.IsMatched || CardModel.IsRevealed)
        {
            return;
        }
        if(gameplayController.IsBusyChecking())
        {
            return;
        }
        CardModel.IsRevealed = true;
        CardView.FlipCard(true);

        gameplayController.OnCardFlipped(this);
    }
    public void ResetCard(Sprite frontSprite)
    {
        CardModel.SetFrontImage(frontSprite);
        CardView.SetFrontSprite(frontSprite);
        CardView.ResetView();
    }
    public void HideCard()
    {
        CardModel.IsRevealed = false;
        CardView.FlipCard(false);
    }
    public void MarkMarched()
    {
        CardModel.IsMatched = true;
    }
}
