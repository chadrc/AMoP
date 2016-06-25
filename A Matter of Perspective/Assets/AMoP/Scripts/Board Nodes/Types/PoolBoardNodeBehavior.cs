using UnityEngine;

public class PoolBoardNodeBehavior : BoardNodeBehavior
{
    [SerializeField]
    new private ParticleSystem particleSystem;

    private ParticleSystem.EmissionModule emission;
    
    protected override void Awake()
    {
        emission = particleSystem.emission;
    }

    public override void SendEnergy(BoardNode to)
    {
        var energy = LevelBehavior.Current.EnergyPoolManager.GetOneEnergy(Node.Affiliation);
        energy.Travel(Node, to);
    }

    protected override void OnNodeAffiliationChanged(BoardNodeAffiliation affiliation)
    {
        ParticleSystem.ColorOverLifetimeModule color = particleSystem.colorOverLifetime;
        color.color = new ParticleSystem.MinMaxGradient(AMoPUtils.GetColorForAffiliation(affiliation));
    }

    protected override void OnNodeEnergyChanged(float energy)
    {
        emission.rate = new ParticleSystem.MinMaxCurve(energy);
    }

    protected override void OnNodeTypeChanged(BoardNodeType type)
    {
    }

    protected override void setAlpha(float a)
    {
        var renderer = particleSystem.gameObject.GetComponent<ParticleSystemRenderer>();
        Color clr = renderer.material.GetColor("_TintColor");
        clr.a = a;
        renderer.material.SetColor("_TintColor", clr);
    }
}
