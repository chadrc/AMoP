using UnityEngine;
using System.Collections;

public class EnergyFactory : ScriptableObject
{
    [SerializeField]
    private GameObject playerEnergy;

    [SerializeField]
    private GameObject enemyEnergy;

    public Energy CreateEnergy(BoardNodeAffiliation affiliation)
    {
        var energyObj = GameObject.Instantiate(GetPrefabByAffiliation(affiliation));
        var behavior = energyObj.GetComponent<EnergyBehavior>();
        var energy = new Energy(behavior);
        behavior.Init(energy);
        
        return energy;
    }

    private GameObject GetPrefabByAffiliation(BoardNodeAffiliation affiliation)
    {
        switch (affiliation)
        {
            case BoardNodeAffiliation.Enemy:
                return playerEnergy;
            case BoardNodeAffiliation.Player:
                return enemyEnergy;
            case BoardNodeAffiliation.Neutral:
                return null;
        }
        return null;
    }
}

