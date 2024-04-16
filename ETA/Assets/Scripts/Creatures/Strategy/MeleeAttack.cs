using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공격 전략: 근접 자동 공격
/// 1. 적과의 거리가 공격 범위보다 작거나 같으면 Idle 상태로 1초
/// 2. 공격 수행
/// </summary>
public class MeleeAttack : IAttackStrategy
{
    // 여기서 상태를 사용할 수 없나?
    public void Attack(GameObject _target)
    {
        Debug.Log("근접 자동 공격");

    }
}
