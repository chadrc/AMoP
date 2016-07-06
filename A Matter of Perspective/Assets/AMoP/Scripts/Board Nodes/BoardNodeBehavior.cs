using UnityEngine;

[RequireComponent (typeof(SphereCollider))]
public abstract class BoardNodeBehavior : MonoBehaviour
{
    public BoardNode Node { get; private set; }
    public SphereCollider EnergyCollider { get; private set; }
    public event System.Action<EnergyBehavior> EnergyEnter;

    public virtual void AttachToNode(BoardNode node)
    {
        DetachFromNode();

        // 2.5 is 3 (half of board size 6) minus .5 (offset to nodes default position at the origin)
        // subtract it from all nodes to center board on screen
        EnergyCollider = GetComponent<SphereCollider>();
        Vector3 pos = node.Position - new Vector3(2.5f, 2.5f, 2.5f);
        transform.position = pos;

        Node = node;
        node.Affiliation.Changed += OnNodeAffiliationChanged;
        node.Type.Changed += OnNodeTypeChanged;
        node.Energy.Changed += OnNodeEnergyChanged;
        OnNodeEnergyChanged(node.Energy);
        OnNodeAffiliationChanged(node.Affiliation);
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

    public void HalfFade()
    {
        setAlpha(.5f);
    }

    public void FullFade()
    {
        setAlpha(0);
    }

    public void NoFade()
    {
        setAlpha(1.0f);
    }

    public abstract void SendEnergy(BoardNode to);
    protected abstract void setAlpha(float a);
    protected abstract void OnNodeAffiliationChanged(BoardNodeAffiliation affiliation);
    protected abstract void OnNodeTypeChanged(BoardNodeType type);
    protected abstract void OnNodeEnergyChanged(float energy);

    // Use this for initialization
    protected virtual void Awake()
    {

    }
    
    void OnDestroy()
    {
        DetachFromNode();
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
