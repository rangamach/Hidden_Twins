using UnityEngine;


public class CardController
{
    public CardView CardView { get; private set; }
    public CardModel CardModel { get; private set; }
    private GameplayController gameplayController;
    public CardController(CardView prefab, Sprite back)
    {
        //Instantiate View and Model
        this.CardView = Object.Instantiate(prefab.gameObject).GetComponent<CardView>();
        this.CardModel = new CardModel(back);

        //Set the back / hidden sprite
        this.CardView.SetBackSprite(this.CardModel.BackImage);

        CardView.SetController(this);
    }
    public void SetGameplayController(GameplayController controller) => this.gameplayController = controller;
    public void OnCardClicked()
    {
        //checks is card is matched already, already revealed or the gameplay controller busy checking other 2 cards
        if ((CardModel.IsMatched || CardModel.IsRevealed) || gameplayController.IsBusyChecking())
        {
            return;
        }
        GameService.Instance.SoundService.PlaySFX(SoundType.Card_Flip);
        CardModel.IsRevealed = true;
        CardView.FlipCard(true);

        gameplayController.OnCardFlipped(this);
    }
    public void ResetCard(Sprite frontSprite)
    {
        CardModel.SetFrontImage(frontSprite);
        CardView.SetFrontSprite(frontSprite);
        CardModel.IsRevealed = false;
        CardModel.IsMatched = false;
        CardView.ResetView();
    }
    public void HideCard()
    {
        CardModel.IsRevealed = false;
        CardView.FlipCard(false);
    }
    public void MarkMatched()
    {
        CardModel.IsMatched = true;
        CardView.FlipCard(true);
    }
}
