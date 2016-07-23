using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoreItemViewController : MonoBehaviour
{
    private GridLayoutGroup grid;

    private void Awake()
    {
        ScreenChangeListeningBehavior.ScreenChanged += onScreenChanged;
        grid = GetComponent<GridLayoutGroup>();
    }

    private void onScreenChanged(int width, int height)
    {
        if (width > height)
        {
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        }
        else
        {
            grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        }
    }
}
