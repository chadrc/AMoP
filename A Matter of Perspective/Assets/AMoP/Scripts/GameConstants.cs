using UnityEngine;

public class GameConstants : ScriptableObject
{
    #region Unity Inspector Fields
    [Header("Node Values")]

    [SerializeField]
    private float nodeMaxEnergy;

    [SerializeField]
    private float drainNodeDepletionRate;

    [SerializeField]
    private float fillNodeFillRequirement;

    [SerializeField]
    private float poolNodeGenerationRate;

    [SerializeField]
    private float vortexNodeDepletionRate;

    #endregion

    #region Properties

    public float NodeMaxEnergy { get { return nodeMaxEnergy; } }
    public float DrainNodeDepletionRate { get { return drainNodeDepletionRate; } }
    public float FillNodeFillRequirement { get { return fillNodeFillRequirement; } }
    public float PoolNodeGenerationRate { get { return poolNodeGenerationRate; } }
    public float VortexNodeDepletionRate { get { return vortexNodeDepletionRate; } }

    #endregion
}
