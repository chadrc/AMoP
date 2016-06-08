using UnityEngine;

public class BoardNodeBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject graphicObject;

    private Vector3 minScale = new Vector3(.25f, .25f, .25f);
    private Vector3 maxScale = new Vector3(.75f, .75f, .75f);

    public BoardNode Node { get; private set; }
    
    public event System.Action<int> EnergyEnter;

    public void Select()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void Deselect()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public void Highlight()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public void Unhighlight()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public void AttachToNode(BoardNode node)
    {
        DetachFromNode();

        // 2.5 is the center of 6 unit spheres
        // subtract it from all nodes to center board on screen
        Vector3 pos = node.Position - new Vector3(2.5f, 2.5f, 2.5f);
        transform.position = pos;

        Node = node;
        node.Affiliation.Changed += OnNodeAffiliationChanged;
        node.Type.Changed += OnNodeTypeChanged;
        node.Energy.Changed += OnNodeEnergyChanged;
        node.EnergyTransfered += OnEnergyTransfered;
        Resize();
    }

    public void DetachFromNode()
    {
        if (Node == null)
        {
            return;
        }

        Node.Affiliation.Changed -= OnNodeAffiliationChanged;
        Node.Type.Changed -= OnNodeTypeChanged;
        Node.Energy.Changed -= OnNodeEnergyChanged;
    }

    private void OnNodeAffiliationChanged(BoardNodeAffiliation affiliation)
    {

    }

    private void OnNodeTypeChanged(BoardNodeType type)
    {

    }

    private void OnNodeEnergyChanged(float energy)
    {
        Resize();
    }

    private void OnEnergyTransfered(BoardNode to, int amount)
    {

    }

    private void Resize()
    {
        graphicObject.transform.localScale = Vector3.Lerp(minScale, maxScale, Node.Energy / 20f);
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
