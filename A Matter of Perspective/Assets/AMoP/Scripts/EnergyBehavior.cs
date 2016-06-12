using UnityEngine;
using System.Collections;

public class EnergyBehavior : MonoBehaviour
{
    public Energy EnergyObj { get; private set; }

    public void Init(Energy energy)
    {
        EnergyObj = energy;
    }

    public void TravelTo(BoardNode to)
    {
        gameObject.SetActive(true);
        StartCoroutine(TravelToNode(to));
    }

    private IEnumerator TravelToNode(BoardNode to)
    {
        var startPos = transform.position;
        float travelTime = .5f;
        float timer = 0;
        float varience = .25f;
        float randX = Random.Range(-varience, varience);
        float randY = Random.Range(-varience, varience);
        float randZ = Random.Range(-varience, varience);
        while (timer <= travelTime)
        {
            timer += Time.deltaTime;
            float frac = timer / travelTime;
            float rad = Mathf.PI * frac;
            float sin = Mathf.Sin(rad/2);
            var pos = Vector3.Lerp(startPos, to.Behavior.transform.position, sin);
            float weight = Mathf.PingPong(timer / (travelTime / 2f), 1.0f);
            pos.x += randX * weight;
            pos.y += randY * weight;
            pos.z += randZ * weight;
            transform.position = pos;
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
    }
}
