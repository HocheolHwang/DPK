using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WhaleAnimationData : MonsterAnimationData
{
    // ---------------------------- Animation Clip Name -----------------------------------
    [Header("Whale Animation Data")]
    // Counter
    [SerializeField] protected string groggyParamName = "GROGGY";
    [SerializeField] protected string counterEnableParamName = "COUNTER_ENABLE";
    [SerializeField] protected string counterAttackParamName = "COUNTER_ATTACK";

    public int GroggyParamHash { get; private set; }
    public int CounterEnableParamHash { get; private set; }
    public int CounterAttackParamHash { get; private set; }

    // ---------------------------- Animation Clip -----------------------------------
    // Counter
    [SerializeField] private AnimationClip counterEnableAnim;
    [SerializeField] private AnimationClip counterAttackAnim;

    public AnimationClip CounterEnableAnim { get => counterEnableAnim; }
    public AnimationClip CounterAttackAnim { get => counterAttackAnim; }


    public override void StringAnimToHash()
    {
        base.StringAnimToHash();
        
        // Counter
        GroggyParamHash = Animator.StringToHash(groggyParamName);
        CounterEnableParamHash = Animator.StringToHash(counterEnableParamName);
        CounterAttackParamHash = Animator.StringToHash(counterAttackParamName);
    }
}
