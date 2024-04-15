using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이동 상태를 나타낸다.
/// </summary>
public class MoveState : State
{
    [SerializeField] public AnimationClip anim;
    [SerializeField] public float maxSpeed;

    public override void Enter()
    {
        animator.Play(anim.name);
    }

    public override void Execute()
    {

        // 사정거리에 플레이어 또는 몬스터가 있는 경우 멈춘다.
        // HP가 0인 경우 멈춘다.
        // isComplete = false;
    }
}
