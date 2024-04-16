using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상태를 관리한다.
/// 다른 종류의 Agent에서 매번 상태 관리 코드를 작성하지 않고, 상태 머신의 instance를 가진다.
/// </summary>
public class StateMachine
{
    // SM이 가진 State
    public State curState { get; protected set; }

    /// <summary>
    /// 현재 상태를 새로운 상태로 변경한다.
    /// </summary>
    public void ChangeState(State newState, bool forceReset)
    {
        Debug.Log($"ChangeState {newState.gameObject.name}");
        if (curState != newState || forceReset)    // 현재 상태와 새로운 상태가 같지 않거나 강제로 상태를 변경하는 경우
        {
            curState?.Exit();
            curState = newState;
            curState.Initialize(this);
            curState.Enter();
        }
    }

    /// <summary>
    /// 현재 상태를 매 프레임마다 실행
    /// </summary>
    public void Execute()
    {
        curState?.Execute();
    }
}
