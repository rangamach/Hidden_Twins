using UnityEngine;
public class CardModel
{
    public Sprite FrontImage { get; private set; }
    public Sprite BackImage { get; private set; }

    public bool IsRevealed { get; set; }
    public bool IsMatched { get; set; }
    public CardModel(Sprite back)
    {
        this.BackImage = back;
    }
    public void SetFrontImage(Sprite sprite) => this.FrontImage = sprite;
}
