using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(CanvasGroup))]
public class OptionsViewController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup mainMenuCanvas;

    [SerializeField]
    private GridLayoutGroup signInButtonGrid;

    [SerializeField]
    private GridLayoutGroup volumeSliderGrid;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        ScreenChangeListeningBehavior.ScreenChanged += onScreenChanged;
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.Hide();
    }

    public void OnMaterVolumeSliderChanged(float value)
    {

    }

    public void OnMusicVolumeSliderChanged(float value)
    {

    }

    public void OnSoundVolumeSliderChanged(float value)
    {

    }

    public void OnCloseButtonPressed()
    {
        canvasGroup.Hide();
        mainMenuCanvas.Show();
    }
    
    private void onScreenChanged(int width, int height)
    {
        if (width > height)
        {
            signInButtonGrid.constraintCount = 1;
            volumeSliderGrid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        }
        else
        {
            signInButtonGrid.constraintCount = 2;
            volumeSliderGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        }
    }
}
