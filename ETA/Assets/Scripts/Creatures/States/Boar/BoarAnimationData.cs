using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Boar Monster의 애니메이션 최적화와 관리를 위한 클래스
[Serializable]
public class BoarAnimationData
{
    [SerializeField] private string idleParamName = "Idle";
    [SerializeField] private string chaseParamName = "Chase";
    [SerializeField] private string attackParamName = "Attack";
    [SerializeField] private string dieParamName = "Die";

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
