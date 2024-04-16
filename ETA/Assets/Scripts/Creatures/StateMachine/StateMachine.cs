using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상태를 관리한다.
/// 다른 종류의 Agent에서 매번 상태 관리 코드를 작성하지 않고, 상태 머신의 instance를 가진다.
/// </summary>
public class StateMachine
{
    public State state; 

    // 현재 상태를 새로운 상태로 변경한다.
    public void ChangeState(State newState, bool forceReset)
    {
        if (state != newState || forceReset)    // 현재 상태와 새로운 상태가 같지 않거나 강제로 상태를 변경하는 경우
        {
            state?.Exit();
            state = newState;
            state.Initialize();
            state.Enter();
        }
    }
}
