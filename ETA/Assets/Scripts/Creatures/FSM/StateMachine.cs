using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State _curState { get; set; }


    public void ChangeState(State newState, bool forceReset)
    {
        if (_curState != newState || forceReset)    // 현재 상태와 새로운 상태가 같지 않거나 강제로 상태를 변경하는 경우
        {
            _curState?.Exit();
            _curState = newState;
            _curState.Initialize();
            _curState.Enter();
        }
    }

    public void Execute()
    {
        _curState?.Execute();
    }

    public void FixedExecute()
    {
        _curState?.FixedExecute();
    }
}
