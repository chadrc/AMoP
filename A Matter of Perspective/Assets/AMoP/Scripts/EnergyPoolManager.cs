using UnityEngine;
using System.Collections.Generic;

public class EnergyPoolManager
{
    private EnergyFactory factory;
    private List<Energy> playerEnergies = new List<Energy>();
    private List<Energy> enemyEnergies = new List<Energy>();

    public EnergyPoolManager(EnergyFactory factory)
    {
        this.factory = factory;
        
        playerEnergies = InitPoolForAffiliation(BoardNodeAffiliation.Player, "Player Energy Pool", "Player Energy");
        enemyEnergies = InitPoolForAffiliation(BoardNodeAffiliation.Enemy, "Enemy Energy Pool", "Enemy Energy");
    }

    public Energy GetEnergy(BoardNodeAffiliation affiliation)
    {
        Energy energy = null;
        switch (affiliation)
        {
            case BoardNodeAffiliation.Player:
                energy = GetInactive(playerEnergies);
                break;
            case BoardNodeAffiliation.Enemy:
                energy = GetInactive(enemyEnergies);
                break;
            case BoardNodeAffiliation.Neutral:
                Debug.LogWarning("Attempting to get neutral energy.");
                return null;
        }
        if (energy == null)
        {
            Debug.LogError("All energy in pool for affiliation " + affiliation + " are active. Pool size increase needed.");
        }
        return null;
    }

    private Energy GetInactive(List<Energy> energies)
    {
        foreach(var energy in energies)
        {
            if (!energy.Behavior.gameObject.activeSelf)
            {
                return energy;
            }
        }
        return null;
    }

    private List<Energy> InitPoolForAffiliation(BoardNodeAffiliation affiliation, string parentName, string energyNameBase)
    {
        List<Energy> energies = new List<Energy>();
        
        var parent = new GameObject();
        parent.name = parentName;

        for (int i = 0; i < 100; i++)
        {
            var energy = factory.CreateEnergy(affiliation);
            energy.Behavior.transform.SetParent(parent.transform);
            energy.Behavior.transform.localScale = Vector3.one;
            energy.Behavior.gameObject.SetActive(false);
            energy.Behavior.name = energyNameBase + " " + i;
            energies.Add(energy);
        }

        return energies;
    }
}
