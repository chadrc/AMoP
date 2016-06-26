using UnityEngine;
using System.Collections;

public class ScreenChangeListeningBehavior : MonoBehaviour
{
    public static event System.Action<int, int> ScreenChanged;

    private int InitialScreenWidth;
    private int InitialScreenHeight;

	// Use this for initialization
	void Awake ()
    {
        InitialScreenHeight = Screen.height;
        InitialScreenWidth = Screen.width;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Screen.width != InitialScreenWidth ||
            Screen.height != InitialScreenHeight)
        {
            InitialScreenHeight = Screen.height;
            InitialScreenWidth = Screen.width;

            if (ScreenChanged != null)
            {
                ScreenChanged(InitialScreenWidth, InitialScreenHeight);
            }
        }
    }
}
