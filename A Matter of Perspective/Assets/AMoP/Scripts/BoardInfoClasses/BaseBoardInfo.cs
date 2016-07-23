using UnityEngine;
using System.Collections;

public abstract class BaseBoardInfo : MonoBehaviour
{
    protected BoardInfoViewController infoController;

	protected virtual void Awake ()
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
	}

    protected virtual void Start ()
    {

    }

    protected virtual void Update()
    {

    }
}
