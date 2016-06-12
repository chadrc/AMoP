using UnityEngine;
using System.Collections;

public class BoardNodeBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject graphicObject;

    public BoardNode Node { get; private set; }

    public event System.Action<EnergyBehavior> EnergyEnter;

    new private MeshRenderer renderer;
    private Vector3 minScale = new Vector3(.25f, .25f, .25f);
    private Vector3 maxScale = new Vector3(.75f, .75f, .75f);

    public void Select()
    {
        //renderer.material.color = Color.red;
    }

    public void Deselect()
    {
        //renderer.material.color = Color.white;
    }

    public void Highlight()
    {
        //renderer.material.color = Color.green;
    }

    public void Unhighlight()
    {
        //renderer.material.color = Color.white;
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
        Resize();
        ChangeColor();
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
        ChangeColor();
    }

    private void OnNodeTypeChanged(BoardNodeType type)
    {

    }

    private void OnNodeEnergyChanged(float energy)
    {
        Resize();
    }

    private void Resize()
    {
        graphicObject.transform.localScale = Vector3.Lerp(minScale, maxScale, Node.Energy / 20f);
    }

    private void ChangeColor()
    {
        switch (Node.Affiliation.Value)
        {
            case BoardNodeAffiliation.Player:
                renderer.material.color = Color.cyan;
                break;

            case BoardNodeAffiliation.Enemy:
                renderer.material.color = new Color(1.0f, 1.0f, 0);
                break;

            case BoardNodeAffiliation.Neutral:
                renderer.material.color = Color.white;
                break;
        }
    }

	// Use this for initialization
	void Awake ()
    {
        renderer = transform.GetChild(0).GetComponent<MeshRenderer>();
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
        var energy = collider.gameObject.GetComponent<EnergyBehavior>();
        if (energy != null && energy.EnergyObj.Origin != Node && EnergyEnter != null)
        {
            EnergyEnter(energy);
        }
    }
}
