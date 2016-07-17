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

    [Header("End Level Animations")]

    [SerializeField]
    private float endAnimationSlowDownEffectTime;

    [SerializeField]
    private float endAnimationPanelFadeInTime;

    [SerializeField]
    private float endAnimationTextAnimationTime;

    [SerializeField]
    private float endAnimationScoreDelay;

    #endregion

    #region Properties

    public float NodeMaxEnergy { get { return nodeMaxEnergy; } }
    public float DrainNodeDepletionRate { get { return drainNodeDepletionRate; } }
    public float FillNodeFillRequirement { get { return fillNodeFillRequirement; } }
    public float PoolNodeGenerationRate { get { return poolNodeGenerationRate; } }
    public float VortexNodeDepletionRate { get { return vortexNodeDepletionRate; } }

    public float EndAnimationSlowDownEffectTime { get { return endAnimationSlowDownEffectTime; } }
    public float EndAnimationPanelFadeInTime { get { return endAnimationPanelFadeInTime; } }
    public float EndAnimationTextAnimationTime { get { return endAnimationTextAnimationTime; } }
    public float EndAnimationScoreDelay { get { return endAnimationScoreDelay; } }

    #endregion
}
