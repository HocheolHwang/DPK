using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State curState { get; set; }


    public void ChangeState(State newState, bool forceReset)
    {
        if (curState != newState || forceReset)    // 현재 상태와 새로운 상태가 같지 않거나 강제로 상태를 변경하는 경우
        {
            curState?.Exit();
            curState = newState;
            curState.Initialize();
            curState.Enter();
        }
    }

    public void Execute()
    {
        curState?.Execute();
    }

    public void FixedExecute()
    {
        curState?.FixedExecute();
    }
}
