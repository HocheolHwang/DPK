using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 일반 몬스터
/// 동작 방식은 Interface로 붙인다.
/// </summary>
public class NormalMonster : StateMachineCore
{
    // Normal Monster만 가진 상태 또는 전략
    public MoveState moveState;

    private void Start()
    {
        InitInstance();
        ChangeState(moveState);
    }

    private void Update()
    {
        
    }
}
