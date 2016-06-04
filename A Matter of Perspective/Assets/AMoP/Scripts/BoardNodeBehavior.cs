using UnityEngine;

public class BoardNodeBehavior : MonoBehaviour
{
    private BoardNode node;
    
    public event System.Action<int> EnergyEnter;

    public void AttachToNode(BoardNode node)
    {
        DetachFromNode();

        Vector3 pos = node.Position;
        // 2.5 is the center of 6 unit spheres
        // subtract it from all nodes to center board on screen
        pos.x -= 2.5f;
        transform.position = pos;

        node.Affiliation.Changed += OnNodeAffiliationChanged;
        node.Type.Changed += OnNodeTypeChanged;
        node.Energy.Changed += OnNodeEnergyChanged;
    }

    public void DetachFromNode()
    {
        if (node == null)
        {
            return;
        }

        node.Affiliation.Changed -= OnNodeAffiliationChanged;
        node.Type.Changed -= OnNodeTypeChanged;
        node.Energy.Changed -= OnNodeEnergyChanged;
    }

    private void OnNodeAffiliationChanged(BoardNodeAffiliation affiliation)
    {

    }

    private void OnNodeTypeChanged(BoardNodeType type)
    {

    }

    private void OnNodeEnergyChanged(float energy)
    {

    }

	// Use this for initialization
	void Start () {
	
	}

    void OnDestroy()
    {
        DetachFromNode();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {

    }
}
