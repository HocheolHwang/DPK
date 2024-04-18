using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StateMachine
{
    public State CurState { get; set; }
    public State GlobalState { get; set; }
    public State PrevState { get; set; }


    public void ChangeState(State newState)
    {
        if (CurState != newState)    // 현재 상태와 새로운 상태가 같지 않거나 강제로 상태를 변경하는 경우
        {
            if (CurState != null)
            {
                CurState.Exit();
                PrevState = CurState;
            }
            CurState = newState;
            CurState.Initialize();
            CurState.Enter();
        }
    }

    public void SetGlobalState(State newState)
    {
        GlobalState = newState;
    }

    public void ChangeToPrevState()
    {
        ChangeState(PrevState);
    }

    public void Execute()
    {
        GlobalState?.Execute();
        CurState?.Execute();
    }

    public void FixedExecute()
    {
        GlobalState?.FixedExecute();
        CurState?.FixedExecute();
    }
}
