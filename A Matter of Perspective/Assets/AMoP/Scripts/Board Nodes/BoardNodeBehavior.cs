using UnityEngine;

[RequireComponent (typeof(SphereCollider))]
public abstract class BoardNodeBehavior : MonoBehaviour
{
    public BoardNode Node { get; private set; }
    public SphereCollider EnergyCollider { get; private set; }
    public event System.Action<EnergyBehavior> EnergyEnter;

    protected bool Hidden { get; private set; }

    public virtual void AttachToNode(BoardNode node)
    {
        DetachFromNode();
        
        EnergyCollider = GetComponent<SphereCollider>();

        Node = node;

        Vector3 pos = node.Position - Node.ParentBoard.OffsetVector;
        transform.position = pos;

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
        Hidden = false;
    }

    public void FullFade()
    {
        setAlpha(0);
        Hidden = true;
    }

    public void NoFade()
    {
        setAlpha(1.0f);
        Hidden = false;
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
