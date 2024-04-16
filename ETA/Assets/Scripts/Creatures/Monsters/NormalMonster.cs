using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 일반 몬스터 : 근거리 공격, 1인 타격, 가장 가까운 적을 타겟팅
/// </summary>
public class NormalMonster : StateMachineCore
{
    // Normal Monster만 가진 상태
    [Header("Having State")]
    public IdleState idleState;
    public MoveState moveState;
    public MeleeState attackState;

    private GameObject target;

    // Normal Monster가 가진 전략
    private IAttackStrategy attackStrategy;

    private const float attackRange = 3.0f;


    private void Start()
    {
        InitInstance();
        ChangeState(idleState);

        attackStrategy = new MeleeAttack();
    }

    private void Update()
    {
        // IsComplete 사용
        SelectorState();
    }

    private void SelectorState()
    {
        if (target == null)
        {
            ChangeState(idleState);
            // 원래 자리로 돌아갈거면 여기서
        }
        else
        {
            // 공격 범위에 없으면
            if (!IsExistTargetFromRange())
            {
                ChangeState(moveState);
            }
            else
            {
                attackStrategy.Attack(target);
            }
        }
    }

    public bool IsExistTargetFromRange()
    {
        return Vector3.Distance(target.transform.position, transform.position) < attackRange;
    }
}
