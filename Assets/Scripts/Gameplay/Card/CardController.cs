using UnityEngine.UI;
using UnityEngine;

public class CardController
{
    public CardView CardView { get; private set; }
    public CardModel CardModel { get; private set; }
    public CardController(CardView prefab, Sprite back)
    {
        this.CardView = Object.Instantiate(prefab.gameObject).GetComponent<CardView>();
        this.CardModel = new CardModel(back);

        CardView.SetController(this);
    }
    public void ResetCard(Sprite frontSprite)
    {
        CardModel.SetFrontImage(frontSprite);
        CardView.SetFrontSprite(frontSprite);
        CardView.ResetView();
    }
}
