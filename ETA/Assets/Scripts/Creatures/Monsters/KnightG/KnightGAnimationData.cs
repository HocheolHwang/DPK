using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KnightGAnimationData : MonsterAnimationData
{
    [Header("KnightG Animation Data")]
    [SerializeField] protected string attackUpParamName = "ATTACK_UP";
    [SerializeField] protected string groggyParamName = "GROGGY";
    public int AttackUpParamHash { get; private set; }
    public int GroggyParamHash { get; private set; }

    // ---------------------------- Animation Clip -----------------------------------
    [SerializeField] private AnimationClip attackUpAnim;
    public AnimationClip AttackUpAnim { get => attackUpAnim; }


    public override void StringAnimToHash()
    {
        base.StringAnimToHash();
        AttackUpParamHash = Animator.StringToHash(attackUpParamName);
        GroggyParamHash = Animator.StringToHash(groggyParamName);
    }
}
