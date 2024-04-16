using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// [ 추적 전략 ]
/// 1. Inspector View에서 이동 방식을 정한다.
/// 2. 목적지를 향해 정해진 이동 방식으로 추격한다.
/// 
/// [ 종료 조건 ]
/// 1. 공격 범위내에 타겟이 있는 경우
/// 2. Core가 사망한 경우
/// </summary>
public class ChaseStrategy : State
{
    [Header("Set the Movement State")]
    [SerializeField] public State animState;

    public override void Enter()
    {
        Debug.Log("Enter ChaseStrategy");
        ChangeState(animState, true);
    }

    public override void Execute()
    {
        if (machineCore.detector.CheckWithinAttackRange())
        {
            isComplete = true;
        }

        // 해당 전략을 가진 Agent가 죽으면 ChangeState(Die)
    }

    public override void Exit()
    {
        Debug.Log("Exit ChaseStrategy");
    }
}
