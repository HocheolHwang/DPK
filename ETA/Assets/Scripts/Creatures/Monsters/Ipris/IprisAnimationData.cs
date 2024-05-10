using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IprisAnimationData : MonsterAnimationData
{
    // ---------------------------- Animation Clip Name -----------------------------------
    [Header("Ipris Animation Data")]
    [SerializeField] protected string idleBattleParamName = "IDLE_BATTLE";
    [SerializeField] protected string groggyParamName = "GROGGY";
    [SerializeField] protected string toDragonParamName = "TO_DRAGON";

    [SerializeField] protected string buffParamName = "BUFF";
    [SerializeField] protected string counterEnableParamName = "COUNTER_ENABLE";
    [SerializeField] protected string counterAttackParamName = "COUNTER_ATTACK";
    [SerializeField] protected string patternOneEnableParamName = "PATTERN_ONE_ENABLE";
    [SerializeField] protected string patternOneParamName = "PATTERN_ONE";
    [SerializeField] protected string patternTwoParamName = "PATTERN_TWO";
    [SerializeField] protected string patternTwoWindMillParamName = "PATTERN_TWO_WINDMILL";

    #region HASH
    public int IdleBattleParamHash { get; private set; }
    public int GroggyParamHash { get; private set; }
    public int ToDragonParamHash { get; private set; }

    public int BuffParamHash { get; private set; }
    public int CounterEnableParamHash { get; private set; }
    public int CounterAttackParamHash { get; private set; }
    public int PatternOneEnableParamHash { get; private set; }
    public int PatternOneParamHash { get; private set; }
    public int PatternTwoParamHash { get; private set; }
    public int PatternTwoWindMillParamHash { get; private set; }

    #endregion

    // ---------------------------- Animation Clip ----------------------------------------
    [SerializeField] private AnimationClip dieAnim;
    [SerializeField] private AnimationClip attackFirstAnim;
    [SerializeField] private AnimationClip attackSecondAnim;
    [SerializeField] private AnimationClip buffAnim;
    [SerializeField] private AnimationClip counterEnableAnim;
    [SerializeField] private AnimationClip counterAttackAnim;
    [SerializeField] private AnimationClip patternOneEnableAnim;
    [SerializeField] private AnimationClip patternOneAnim;
    [SerializeField] private AnimationClip patternTwoAnim;
    [SerializeField] private AnimationClip patternTwoWindMillAnim;

    #region SET/GET Anim
    public AnimationClip DieAnim { get => dieAnim; }
    public AnimationClip AttackFirstAnim { get => attackFirstAnim; }
    public AnimationClip AttackSecondAnim { get => attackSecondAnim; }
    public AnimationClip BuffAnim { get => buffAnim; }
    public AnimationClip CounterEnableAnim { get => counterEnableAnim; }
    public AnimationClip CounterAttackAnim { get => counterAttackAnim; }
    public AnimationClip PatternOneEnableAnim { get => patternOneEnableAnim; }
    public AnimationClip PatternOneAnim { get => patternOneAnim; }
    public AnimationClip PatternTwoAnim { get => patternTwoAnim; }
    public AnimationClip PatternTwoWindMillAnim { get => patternTwoWindMillAnim; }

    #endregion

    public override void StringAnimToHash()
    {
        base.StringAnimToHash();

        IdleBattleParamHash = Animator.StringToHash(idleBattleParamName);
        GroggyParamHash = Animator.StringToHash(groggyParamName);
        ToDragonParamHash = Animator.StringToHash(toDragonParamName);

        BuffParamHash = Animator.StringToHash(buffParamName);
        CounterEnableParamHash = Animator.StringToHash(counterEnableParamName);
        CounterAttackParamHash = Animator.StringToHash(counterAttackParamName);
        PatternOneEnableParamHash = Animator.StringToHash(patternOneEnableParamName);
        PatternOneParamHash = Animator.StringToHash(patternOneParamName);
        PatternTwoParamHash = Animator.StringToHash(patternTwoParamName);
        PatternTwoWindMillParamHash = Animator.StringToHash(patternTwoWindMillParamName);
    }
}
