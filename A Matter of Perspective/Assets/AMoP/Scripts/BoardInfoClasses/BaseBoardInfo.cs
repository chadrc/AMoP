using UnityEngine;
using System.Collections;

public abstract class BaseBoardInfo : MonoBehaviour
{
    protected BoardInfoViewController infoController;

	private void Awake ()
    {
        var obj = GameObject.Find("BoardInfoTools");
        if (obj == null)
        {
            Debug.LogError("No BoardInfoTools object in scene.");
        } 
        else
        {
            infoController = obj.GetComponent<BoardInfoViewController>();
            if (infoController == null)
            {
                Debug.LogError("No BoardInfoViewController attached to BoardInfoTools object.");
            }
        }

        LevelBehavior.GameEnd += onGameEnd;
	}

    private void Start ()
    {
        StartCoroutine(setupRoutine());
    }

    private void onGameEnd()
    {
        destroy();
        LevelBehavior.GameEnd -= onGameEnd;
    }

    private IEnumerator setupRoutine()
    {
        // Accounting for Node button panels wait for end of frame in its init
        // that is waiting for camera to update
        yield return new WaitForEndOfFrame();
        setup();
    }

    protected abstract void destroy();
    protected abstract void setup();
}
