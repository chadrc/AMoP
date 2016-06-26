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

    public void HideAllEnergy()
    {
        foreach(var e in playerEnergies)
        {
            e.Behavior.gameObject.SetActive(false);
        }

        foreach (var e in enemyEnergies)
        {
            e.Behavior.gameObject.SetActive(false);
        }
    }

    public List<Energy> GetEnergy(BoardNodeAffiliation affiliation, int amount)
    {
        if (amount == 0)
        {
            return new List<Energy>();
        }

        List<Energy> energies = null;
        switch (affiliation)
        {
            case BoardNodeAffiliation.Player:
                energies = GetInactive(playerEnergies, amount);
                break;
            case BoardNodeAffiliation.Enemy:
                energies = GetInactive(enemyEnergies, amount);
                break;
            case BoardNodeAffiliation.Neutral:
                Debug.LogWarning("Attempting to get neutral energy.");
                return null;
        }
        if (energies.Count == 0)
        {
            Debug.LogError("All energy in pool for affiliation " + affiliation + " are active. Pool size increase needed.");
        }
        return energies;
    }

    public Energy GetOneEnergy(BoardNodeAffiliation affiliation)
    {
        var list = GetEnergy(affiliation, 1);
        return list.Count == 0 ? null : list[0];
    }

    private List<Energy> GetInactive(List<Energy> energies, int amount)
    {
        List<Energy> inactiveEnergies = new List<Energy>();
        foreach(var energy in energies)
        {
            if (!energy.Behavior.gameObject.activeSelf)
            {
                if (inactiveEnergies.Count < amount)
                {
                    inactiveEnergies.Add(energy);
                }
                else
                {
                    break;
                }
            }
        }
        return inactiveEnergies;
    }

    private Energy GetFirstInactive(List<Energy> energies)
    {
        var list = GetInactive(energies, 1);
        return list.Count == 0 ? null : list[0];
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
