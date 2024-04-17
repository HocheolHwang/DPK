using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 일반 몬스터 : 근거리 공격, 1인 타격, 가장 가까운 적을 타겟팅
/// </summary>
public class NormalMonster : StateMachineCore
{
    // Normal Monster만 가진 상태 또는 전략
    [Header("set strategy")]
    public State idleState;
    public State chaseStrategy;
    public State attackStrategy;


    private void Start()
    {
        InitInstance();
        ChangeState(idleState);
    }

    private void Update()
    {
        StateSelector();

        machine.Execute();
    }

    private void FixedUpdate()
    {
        machine.FixedExecute();
    }

    private void StateSelector()
    {
        // 현재 상태가 끝난 경우
        // Idle은 target이 존재하면 isComplete
        if (machine.curState.isComplete)
        {
            // idle 상태가 끝난 경우
            if (machine.curState == idleState)
            {
                ChangeState(chaseStrategy);
            }
            // 공격 전략이 끝난 경우
            else if (machine.curState == attackStrategy)
            {
                ChangeState(chaseStrategy);
            }
        }

        // 추격 전략을 수행하고 있는 경우
        if (machine.curState == chaseStrategy)
        {
            // 공격 범위에 적이 있으면 공격한다.
            if (/*detector.CheckWithinAttackRange()*/IsArriveAgent())
            {
                ChangeState(attackStrategy);
            }
        }

        // 적이 없는 경우
        if (detector.target == null)
        {
            ChangeState(idleState);
        }

    }

    
}
