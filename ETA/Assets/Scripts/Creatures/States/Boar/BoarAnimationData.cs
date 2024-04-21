using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Boar Monster의 애니메이션 최적화를 위한 클래스
[Serializable]
public class BoarAnimationData
{
    [SerializeField] private string idleParamName = "Idle";
    [SerializeField] private string chaseParamName = "Chase";
    [SerializeField] private string attackParamName = "Attack";
    [SerializeField] private string damageParamName = "Damage";
    [SerializeField] private string dieParamName = "Die";

    public int IdleParamHash { get; private set; }
    public int ChaseParamHash { get; private set; }
    public int AttackParamHash { get; private set; }
    public int DamageParamHash { get; private set; }
    public int DieParamHash { get; private set; }

    public void StringAnimToHash()
    {
        IdleParamHash = Animator.StringToHash(idleParamName);
        ChaseParamHash = Animator.StringToHash(chaseParamName);
        AttackParamHash = Animator.StringToHash(attackParamName);
        DamageParamHash = Animator.StringToHash(damageParamName);
        DieParamHash = Animator.StringToHash(dieParamName);
    }
}
