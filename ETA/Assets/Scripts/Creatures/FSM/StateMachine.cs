using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StateMachine
{
    public State _curState { get; set; }
    public State _globalState { get; set; }
    public State _prevState { get; set; }


    public void ChangeState(State newState)
    {
        if (_curState != newState)    // 현재 상태와 새로운 상태가 같지 않거나 강제로 상태를 변경하는 경우
        {
            if (_curState != null)
            {
                _curState.Exit();
                _prevState = _curState;
            }
            _curState = newState;
            _curState.Initialize();
            _curState.Enter();
        }
    }

    public void SetGlobalState(State newState)
    {
        _globalState = newState;
    }

    public void ChangeToPrevState()
    {
        ChangeState(_prevState);
    }

    public void Execute()
    {
        _globalState?.Execute();
        _curState?.Execute();
    }

    public void FixedExecute()
    {
        _curState?.FixedExecute();
    }
}
