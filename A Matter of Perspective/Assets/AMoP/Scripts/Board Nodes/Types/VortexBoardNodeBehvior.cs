using UnityEngine;
using System.Collections;
using System;

public class VortexBoardNodeBehvior : BoardNodeBehavior
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
        // Can't send energy
    }

    protected override void OnNodeAffiliationChanged(BoardNodeAffiliation affiliation)
    {
        ParticleSystem.ColorOverLifetimeModule color = particleSystem.colorOverLifetime;
        color.color = new ParticleSystem.MinMaxGradient(AMoPUtils.GetColorForAffiliation(affiliation));
    }

    protected override void OnNodeEnergyChanged(float energy)
    {
        emission.rate = new ParticleSystem.MinMaxCurve(Mathf.Clamp(energy, 1.0f, 20f));
    }

    protected override void OnNodeTypeChanged(BoardNodeType type)
    {

    }

    protected override void setAlpha(float a)
    {

    }
}
