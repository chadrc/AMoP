using UnityEngine;

public class VortexBoardNode : BoardNode
{
    private float depletionRate = GameData.Constants.VortexNodeDepletionRate;

    public VortexBoardNode(BoardNodeData data) : base(data)
    {
    }

    public override bool CanReceive
    {
        get
        {
            return true;
        }
    }

    public override bool CanSend
    {
        get
        {
            return false;
        }
    }

    protected override void Update()
    {
        float energy = Energy;
        energy -= Time.deltaTime * depletionRate;
        if (energy < 0)
        {
            energy = 0;
        }
        Energy.Value = energy;
    }

    protected override void OnEnergyEnter(EnergyBehavior energyBehavior)
    {
        base.OnEnergyEnter(energyBehavior);

        if (Energy == MaxEnergy)
        {
            // Create Basic Node
            var data = new BoardNodeData();
            data.Affiliation = Affiliation;
            data.Type = BoardNodeType.Basic;
            data.Position = Position;
            data.StartingEnergy = MaxEnergy;
            ParentBoard.ReplaceNode(this, data);
            Behavior.gameObject.SetActive(false);
        }
    }
}
