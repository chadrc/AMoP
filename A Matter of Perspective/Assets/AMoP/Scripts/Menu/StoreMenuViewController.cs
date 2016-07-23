using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(CanvasGroup))]
public class StoreMenuViewController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup mainMenuCanvasGroup;

    [SerializeField]
    private GridLayoutGroup itemGrid;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        ScreenChangeListeningBehavior.ScreenChanged += onScreenChanged;
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.Hide();
    }

    public void OnCloseButtonPressed()
    {
        canvasGroup.Hide();
        mainMenuCanvasGroup.Show();
    }

    private void onScreenChanged(int width, int height)
    {
        if (width > height)
        {
            itemGrid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            itemGrid.cellSize = new Vector2(250, 500);
        }
        else
        {
            itemGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            itemGrid.cellSize = new Vector2(650, 250);
        }
    }
}
