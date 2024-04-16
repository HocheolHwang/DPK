using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [ 공격 전략: 근접 자동 공격 ]
/// 1. Idle 상태로 1초
/// 2. 공격 수행
/// 
/// [ 종료 조건 ]
/// 1. 공격 범위에 적이 없다.
/// 2. 적이 null이다.
/// 3. Core가 사망한 경우
/// </summary>
public class MeleeAttackStrategy : State
{
    [Header("States Script")]
    [SerializeField] public IdleState idleState;
    [SerializeField] public MeleeAttackState meleeAttackState;

    public override void Enter()
    {
        Debug.Log("Enter MeleeAttackStrategy");
        ChangeState(idleState);
    }

    public override void Execute()
    {
        if (machineCore.machine.curState == meleeAttackState)
        {
            // 공격 범위에 적이 없으면 공격 전략을 종료한다.
            if ( !machineCore.detector.CheckWithinAttackRange() )
            {
                isComplete = true;
            }
            else if (meleeAttackState.isComplete)
            {
                ChangeState(idleState);
            }
        }
        else
        {
            if (machineCore.machine.curState.time > 1.0f)
            {
                ChangeState(meleeAttackState, true);
            }
        }

        if (machineCore.detector.target == null)
        {
            isComplete = true;
        }

        // 해당 전략을 가진 Agent가 죽으면 ChangeState(Die)
    }

    public override void Exit()
    {
        Debug.Log("Exit MeleeAttackStrategy");
    }

}
