using UnityEngine;

public class PoolBoardNode : BoardNode
{
    private float regenRate = GameData.Constants.PoolNodeGenerationRate;

    public PoolBoardNode(BoardNodeData data, Board parent) : base(data, parent)
    {

    }

    public override bool CanReceive
    {
        get
        {
            return false;
        }
    }

    public override bool CanSend
    {
        get
        {
            return true;
        }
    }

    protected override void Update()
    {
        float energy = Energy;
        energy += Time.deltaTime * regenRate;
        if (energy > MaxEnergy)
        {
            energy = MaxEnergy;
        }
        Energy.Value = energy;
    }
}
