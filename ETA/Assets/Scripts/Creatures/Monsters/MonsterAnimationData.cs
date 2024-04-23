using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterAnimationData
{
    [SerializeField] protected string idleParamName = "Idle";
    [SerializeField] protected string chaseParamName = "Chase";
    [SerializeField] protected string attackParamName = "Attack";
    [SerializeField] protected string dieParamName = "Die";

    // 각 몬스터가 가지는 animation clip을 Inspector View에서 세팅
    [SerializeField] private AnimationClip attackAnim;
    
    public int IdleParamHash { get; private set; }
    public int ChaseParamHash { get; private set; }
    public int AttackParamHash { get; private set; }
    public int DieParamHash { get; private set; }
    public AnimationClip AttackAnim { get => attackAnim; }

    public void StringAnimToHash()
    {
        IdleParamHash = Animator.StringToHash(idleParamName);
        ChaseParamHash = Animator.StringToHash(chaseParamName);
        AttackParamHash = Animator.StringToHash(attackParamName);
        DieParamHash = Animator.StringToHash(dieParamName);
    }
}
