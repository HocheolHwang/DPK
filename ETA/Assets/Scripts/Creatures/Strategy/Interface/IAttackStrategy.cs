using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AttackStrategy 동작 방식 정의
/// </summary>
public interface IAttackStrategy
{
    void Attack(GameObject _target);
}
