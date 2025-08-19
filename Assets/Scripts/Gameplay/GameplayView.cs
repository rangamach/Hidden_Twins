using UnityEngine;
using UnityEngine.UI;

public class GameplayView : MonoBehaviour
{
    private GameplayController gameplayController;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;

    [SerializeField] private float cellArea;
    public void PlaceCards(CardView card, int gridSize)
    {
        card.transform.SetParent(gridLayoutGroup.transform,false);

        float cellSize = cellArea / gridSize;
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
    }
    public void AdjustGrid(int gridSize)
    {
        float cellSize = cellArea / gridSize;
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = gridSize;
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
    }
    public void SetController(GameplayController controller) => this.gameplayController = controller;
}
