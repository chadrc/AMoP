using UnityEngine;
using System.Collections;

public class BoardBehavior : MonoBehaviour {
    private bool canSpin = true;

    public event System.Action SpinEnd;
    public Board BoardObject { get; private set; }

    public void Init(Board board)
    {
        BoardObject = board;
    }

    public void Spin(Vector2 dir)
    {
        if (canSpin)
        {
            StartCoroutine(boardSpin(dir));
        }
    }

    IEnumerator boardSpin(Vector2 dir)
    {
        canSpin = false;
        float spinTime = .25f;
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

        canSpin = true;
        if (SpinEnd != null)
        {
            SpinEnd();
        }
    }
}
