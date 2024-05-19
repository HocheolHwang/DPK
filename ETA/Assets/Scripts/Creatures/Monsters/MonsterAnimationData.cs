using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterAnimationData : MonoBehaviour
{
    [Header("Monster Animation Data")]
    [SerializeField] protected string idleParamName = "IDLE";
    [SerializeField] protected string chaseParamName = "CHASE";
    [SerializeField] protected string attackParamName = "ATTACK";
    [SerializeField] protected string dieParamName = "DIE";

    // 각 몬스터가 가지는 animation clip을 Inspector View에서 세팅
    [SerializeField] private AnimationClip attackAnim;
    
    public int IdleParamHash { get; private set; }
    public int ChaseParamHash { get; private set; }
    public int AttackParamHash { get; private set; }
    public int DieParamHash { get; private set; }
    public AnimationClip AttackAnim { get => attackAnim; }

    public virtual void StringAnimToHash()
    {
        IdleParamHash = Animator.StringToHash(idleParamName);
        ChaseParamHash = Animator.StringToHash(chaseParamName);
        AttackParamHash = Animator.StringToHash(attackParamName);
        DieParamHash = Animator.StringToHash(dieParamName);
    }
}
