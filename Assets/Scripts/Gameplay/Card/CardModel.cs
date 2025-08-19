using UnityEngine;
public class CardModel
{
    public Sprite FrontImage { get; private set; }
    public Sprite BackImage { get; private set; }
    public CardModel(Sprite back)
    {
        //this.FrontImage = front;
        this.BackImage = back;
    }
    public void SetFrontImage(Sprite sprite) => this.FrontImage = sprite;
}
