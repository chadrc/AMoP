using UnityEngine;
using System.Collections;

public class BoardBehavior : MonoBehaviour {
    private bool canSpin = true;

    public event System.Action SpinEnd;
    public Board BoardObject { get; private set; }

    public void Init(Board board)
    {
        BoardObject = board;
        HideShowNodes();
    }

    public void Uninit()
    {
        BoardObject = null;
    }

    public void Spin(Vector2 dir)
    {
        if (canSpin)
        {
            foreach (var node in BoardObject)
            {
                node.Behavior.HalfFade();
            }
            StartCoroutine(boardSpin(dir));
        }
    }

    private void HideShowNodes()
    {
        for (int i = 0; i < BoardObject.BoardSize; i++)
        {
            for (int j = 0; j < BoardObject.BoardSize; j++)
            {
                var boardRow = BoardObject.GetNodeRow(i, j);
                if (boardRow.Closest != null)
                {
                    boardRow.Closest.Behavior.NoFade();
                    boardRow.Closest.Enable();
                }

                foreach (var node in boardRow.Hidden)
                {
                    node.Behavior.FullFade();
                    node.Disable();
                }
            }
        }
    }

    IEnumerator boardSpin(Vector2 dir)
    {
        canSpin = false;
        float spinTime = GameData.Constants.BoardSpinTime;
        float t = 0;
        Quaternion startRot = transform.rotation;
        transform.Rotate(Vector3.up, 90f * -dir.x, Space.World);
        transform.Rotate(Vector3.right, 90f * dir.y, Space.World);
        Quaternion endRot = transform.rotation;
        transform.rotation = startRot;

        while (t < spinTime)
        {
            t += Time.deltaTime;
            float frac = Mathf.Clamp01(t / spinTime);
            transform.rotation = Quaternion.Slerp(startRot, endRot, frac);

            yield return new WaitForEndOfFrame();
        }

        HideShowNodes();

        canSpin = true;
        if (SpinEnd != null)
        {
            SpinEnd();
        }
    }
}
