using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAnimationData : MonoBehaviour
{
    #region ACTION LIST
    [Header("Dragon Basic Action")]
    [SerializeField] protected string groggyParamName = "GROGGY";
    [SerializeField] protected string dieParamName = "DIE";
    [SerializeField] protected string idleParamName = "IDLE";
    [SerializeField] protected string idleBattleParamName = "IDLE_BATTLE";
    [SerializeField] protected string chaseParamName = "CHASE";
    [SerializeField] protected string downAttackParamName = "DOWN_ATTACK";
    [SerializeField] protected string swingAttackParamName = "SWING_ATTACK";
    [SerializeField] protected string tailAttackParamName = "TAIL_ATTACK";

    [Header("Dragon Pattern Action")]
    [SerializeField] protected string fearEnableParamName = "FEAR_ENABLE";
    [SerializeField] protected string fearAttackParamName = "FEAR_ATTACK";
    [SerializeField] protected string breathEnableParamName = "BREATH_ENABLE";
    [SerializeField] protected string breathParamName = "BREATH";
    [SerializeField] protected string cryToDownName = "CRY_TO_DOWN";
    [SerializeField] protected string cryToFireName = "CRY_TO_FIRE";
    [SerializeField] protected string groundToSkyName = "GROUND_TO_SKY";
    [SerializeField] protected string skyDownAttackName = "SKY_DOWN_ATTACK";
    [SerializeField] protected string flyFireBallName = "FLY_FIRE_BALL";
    #endregion

    #region HASH
    public int GroggyParamHash { get; private set; }
    public int DieParamHash { get; private set; }
    public int IdleParamHash { get; private set; }
    public int IdleBattleParamHash { get; private set; }
    public int ChaseParamHash { get; private set; }
    public int DownAttackParamHash { get; private set; }
    public int SwingAttackParamHash { get; private set; }
    public int TailAttackParamHash { get; private set; }
    public int FearEnableParamHash { get; private set; }
    public int FearAttackParamHash { get; private set; }
    public int BreathEnableParamHash { get; private set; }
    public int BreathParamHash { get; private set; }
    public int CryToDownParamHash { get; private set; }
    public int CryToFireParamHash { get; private set; }
    public int GroundToSkyParamHash { get; private set; }
    public int SkyDownAttackParamHash { get; private set; }
    public int FlyFireBallParamHash { get; private set; }
    #endregion

    #region Anim List
    [Header("Animation Clip For State")]
    [SerializeField] private AnimationClip groggyAnim;
    [SerializeField] private AnimationClip dieAnim;
    [SerializeField] private AnimationClip idleAnim;
    [SerializeField] private AnimationClip idleBattleAnim;
    [SerializeField] private AnimationClip chaseAnim;
    [SerializeField] private AnimationClip downAttackAnim;
    [SerializeField] private AnimationClip swingAttackAnim;
    [SerializeField] private AnimationClip tailAttackAnim;
    [SerializeField] private AnimationClip fearEnableAnim;
    [SerializeField] private AnimationClip fearAttackAnim;
    [SerializeField] private AnimationClip breathEnableAnim;
    [SerializeField] private AnimationClip breathAnim;
    [SerializeField] private AnimationClip cryToDownAnim;
    [SerializeField] private AnimationClip cryToFireAnim;
    [SerializeField] private AnimationClip groundToSkyAnim;
    [SerializeField] private AnimationClip skyDownAttackAnim;
    [SerializeField] private AnimationClip flyFireBallAnim;
    #endregion

    #region GET Anim
    public AnimationClip GroggyAnim { get => dieAnim; }
    public AnimationClip DieAnim { get => dieAnim; }
    public AnimationClip IdleAnim { get => dieAnim; }
    public AnimationClip IdleBattleAnim { get => idleBattleAnim; }
    public AnimationClip ChaseAnim { get => chaseAnim; }
    public AnimationClip DownAttackAnim { get => downAttackAnim; }
    public AnimationClip SwingAttackAnim { get => swingAttackAnim; }
    public AnimationClip TailAttackAnim { get => tailAttackAnim; }
    public AnimationClip FearEnableAnim { get => fearEnableAnim; }
    public AnimationClip FearAttackAnim { get => fearAttackAnim; }
    public AnimationClip BreathEnableAnim { get => breathEnableAnim; }
    public AnimationClip BreathAnim { get => breathAnim; }
    public AnimationClip CryToDownAnim { get => cryToDownAnim; }
    public AnimationClip CryToFireAnim { get => cryToFireAnim; }
    public AnimationClip GroundToSkyAnim { get => groundToSkyAnim; }
    public AnimationClip SkyDownAttackAnim { get => skyDownAttackAnim; }
    public AnimationClip FlyFireBallAnim { get => flyFireBallAnim; }
    #endregion

    public void StringAnimToHash()
    {
        GroggyParamHash = Animator.StringToHash(groggyParamName);
        DieParamHash = Animator.StringToHash(dieParamName);
        IdleParamHash = Animator.StringToHash(idleParamName);
        IdleBattleParamHash = Animator.StringToHash(idleBattleParamName);
        ChaseParamHash = Animator.StringToHash(chaseParamName);
        DownAttackParamHash = Animator.StringToHash(downAttackParamName);
        SwingAttackParamHash = Animator.StringToHash(swingAttackParamName);
        TailAttackParamHash = Animator.StringToHash(tailAttackParamName);

        FearEnableParamHash = Animator.StringToHash(fearEnableParamName);
        FearAttackParamHash = Animator.StringToHash(fearAttackParamName);
        BreathEnableParamHash = Animator.StringToHash(breathEnableParamName);
        BreathParamHash = Animator.StringToHash(breathParamName);
        CryToDownParamHash = Animator.StringToHash(cryToDownName);
        CryToFireParamHash = Animator.StringToHash(cryToFireName);
        GroundToSkyParamHash = Animator.StringToHash(groundToSkyName);
        SkyDownAttackParamHash = Animator.StringToHash(skyDownAttackName);
        FlyFireBallParamHash = Animator.StringToHash(flyFireBallName);
    }
}
