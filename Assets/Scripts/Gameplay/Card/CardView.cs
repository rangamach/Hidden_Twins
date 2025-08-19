using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField, Range(0.05f, 0.5f)] private float flipDuration;

    private CardController cardController; 

    private Button cardButton;
    private int pairID = -1;
    private bool isRevealed = false;
    private bool isMatched = false;
    private bool isBusy = false;

    private Image frontImage;
    private Image backImage;

    private void Awake()
    {
        cardButton = GetComponent<Button>();

        frontImage = transform.Find("Front").GetComponent<Image>();
        backImage = transform.Find("Back").GetComponent<Image>();
        
    }
    public void ResetView()
    {
        isRevealed = false;
        isMatched = false;
        frontImage.enabled = false;
        backImage.enabled = true;
    }
    public void SetFrontSprite(Sprite sprite) => this.frontImage.sprite = sprite;
    public void SetController(CardController controller) => this.cardController = controller;
}
