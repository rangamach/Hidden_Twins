using System.Collections;
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
        cardButton.onClick.AddListener(OnCardClick);

        frontImage = transform.Find("Front").GetComponent<Image>();
        backImage = transform.Find("Back").GetComponent<Image>();
        
    }
    private void OnCardClick()
    {
        if(isBusy || isMatched)
        {
            return;
        }
        cardController.OnCardClicked();
    }
    public void ResetView()
    {
        isRevealed = false;
        isMatched = false;
        frontImage.enabled = false;
        backImage.enabled = true;
    }
    public void SetCardSize(Vector2 size)
    {
        frontImage.transform.localScale = size;
        backImage.transform.localScale = size;
    }
    public void SetFrontSprite(Sprite sprite) => this.frontImage.sprite = sprite;
    public void SetBackSprite(Sprite sprite) => this.backImage.sprite = sprite;  
    public void SetController(CardController controller) => this.cardController = controller;

    public void FlipCard(bool showFront)
    {
        if (isBusy) return;
        StartCoroutine(FlipRoutine(showFront));
    }
    //this coroutine does the flipping of the card
    private IEnumerator FlipRoutine(bool showFront)
    {
        isBusy = true;
        float time = 0;
        Vector3 initialscale = transform.localScale;

        while (time < flipDuration)
        {
            time += Time.deltaTime;
            float scaleX = Mathf.Lerp(initialscale.x, 0f, time / flipDuration);
            transform.localScale = new Vector3(scaleX, initialscale.y, initialscale.z);
            yield return null;
        }

        frontImage.enabled = showFront;
        backImage.enabled = !showFront;
        isRevealed = showFront;

        time = 0;
        while (time < flipDuration)
        {
            time += Time.deltaTime;
            float scaleX = Mathf.Lerp(0f, initialscale.x, time / flipDuration);
            transform.localScale = new Vector3(scaleX, initialscale.y, initialscale.z);
            yield return null;
        }
        isBusy = false;
    }
}
