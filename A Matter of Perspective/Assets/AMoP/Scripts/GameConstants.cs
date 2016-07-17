using UnityEngine;

public class GameConstants : ScriptableObject
{
    #region Unity Inspector Fields

    [Header("Scoring")]

    [SerializeField]
    private float gameTimeWeight;

    [SerializeField]
    private float boardTurnsWeight;

    [SerializeField]
    private float energyTransfersWeight;

    [SerializeField]
    private float scoreMultiplier;

    [Header("Orthographic Sizes")]

    [SerializeField]
    private float orthoSizeForSize3;

    [SerializeField]
    private float orthoSizeForSize4;

    [SerializeField]
    private float orthoSizeForSize5;

    [SerializeField]
    private float orthoSizeForSize6;

    [Header("Board Values")]

    [SerializeField]
    private float boardSpinTime;

    [Header("Node Values")]

    [SerializeField]
    private float nodeSendEnergyInterval;

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

    public float GameTimeWeight { get { return gameTimeWeight; } }
    public float BoardTurnsWeight { get { return boardTurnsWeight; } }
    public float EnergyTransfersWeight { get { return energyTransfersWeight; } }
    public float ScoreMultiplier { get { return scoreMultiplier; } }

    public float OrthoSizeFor3 { get { return orthoSizeForSize3; } }
    public float OrthoSizeFor4 { get { return orthoSizeForSize4; } }
    public float OrthoSizeFor5 { get { return orthoSizeForSize5; } }
    public float OrthoSizeFor6 { get { return orthoSizeForSize6; } }

    public float BoardSpinTime { get { return boardSpinTime; } }

    public float NodeSendEnergyInterval { get { return nodeSendEnergyInterval; } }
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
