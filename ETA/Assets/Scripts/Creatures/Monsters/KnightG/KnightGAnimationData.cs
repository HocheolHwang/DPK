using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KnightGAnimationData : MonsterAnimationData
{
    // ---------------------------- Animation Clip Name -----------------------------------
    [Header("KnightG Animation Data")]
    [SerializeField] protected string attackUpParamName = "ATTACK_UP";
    // Skill
    [SerializeField] protected string twoSkillTransitionParamName = "TWO_SKILL_TRANSITION";
    [SerializeField] protected string twoSkillEnergyParamName = "TWO_SKILL_ENERGY";
    [SerializeField] protected string twoSkillAttackParamName = "TWO_SKILL_ATTACK";
    // Counter
    [SerializeField] protected string groggyParamName = "GROGGY";
    [SerializeField] protected string counterEnableParamName = "COUNTER_ENABLE";
    [SerializeField] protected string counterAttackParamName = "COUNTER_ATTACK";
    // Phase
    [SerializeField] protected string phaseTransitionParamName = "PHASE_TRANSITION";
    [SerializeField] protected string phaseAttackParamName = "PHASE_ATTACK";
    [SerializeField] protected string phaseAttackingkParamName = "PHASE_ATTACK_ING";

    public int AttackUpParamHash { get; private set; }
    public int TwoSkillTransitionParamHash { get; private set; }
    public int TwoSkillEnergyParamHash { get; private set; }
    public int TwoSkillAttackParamHash { get; private set; }
    public int GroggyParamHash { get; private set; }
    public int CounterEnableParamHash { get; private set; }
    public int CounterAttackParamHash { get; private set; }
    public int PhaseTransitionParamHash { get; private set; }
    public int PhaseAttackParamHash { get; private set; }
    public int PhaseAttackingParamHash { get; private set; }

    // ---------------------------- Animation Clip -----------------------------------
    [SerializeField] private AnimationClip attackUpAnim;
    //Skill
    [SerializeField] private AnimationClip twoSkillTransitionAnim;
    [SerializeField] private AnimationClip twoSkillEnergyAnim;
    [SerializeField] private AnimationClip twoSkillAttackAnim;

    // Counter
    [SerializeField] private AnimationClip counterEnableAnim;
    [SerializeField] private AnimationClip counterAttackAnim;
    // Phase
    [SerializeField] private AnimationClip phaseTransitionAnim;
    [SerializeField] private AnimationClip phaseAttackAnim;
    [SerializeField] private AnimationClip phaseAttackingAnim;

    public AnimationClip AttackUpAnim { get => attackUpAnim; }
    public AnimationClip TwoSkillTransitionAnim { get => twoSkillTransitionAnim; }
    public AnimationClip TwoSkillEnergyAnim { get => twoSkillEnergyAnim; }
    public AnimationClip TwoSkillAttackAnim { get => twoSkillAttackAnim; }
    public AnimationClip CounterEnableAnim { get => counterEnableAnim; }
    public AnimationClip CounterAttackAnim { get => counterAttackAnim; }
    public AnimationClip PhaseTransitionAnim { get => phaseTransitionAnim; }
    public AnimationClip PhaseAttackAnim { get => phaseAttackAnim; }
    public AnimationClip PhaseAttackingAnim { get => phaseAttackingAnim; }


    public override void StringAnimToHash()
    {
        base.StringAnimToHash();
        
        AttackUpParamHash = Animator.StringToHash(attackUpParamName);
        // Skill
        TwoSkillTransitionParamHash = Animator.StringToHash(twoSkillTransitionParamName);
        TwoSkillEnergyParamHash = Animator.StringToHash(twoSkillEnergyParamName);
        TwoSkillAttackParamHash = Animator.StringToHash(twoSkillAttackParamName);
        // Counter
        GroggyParamHash = Animator.StringToHash(groggyParamName);
        CounterEnableParamHash = Animator.StringToHash(counterEnableParamName);
        CounterAttackParamHash = Animator.StringToHash(counterAttackParamName);
        // Phase
        PhaseTransitionParamHash = Animator.StringToHash(phaseTransitionParamName);
        PhaseAttackParamHash = Animator.StringToHash(phaseAttackParamName);
        PhaseAttackingParamHash = Animator.StringToHash(phaseAttackingkParamName);
    }
}
