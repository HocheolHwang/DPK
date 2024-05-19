using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StateMachine
{
    public State CurState { get; set; }
    public State GlobalState { get; set; }
    public State PrevState { get; set; }


    public void ChangeState(State newState, bool forceReset = false)
    {
        // 현재 상태와 새로운 상태가 같지 않아야 한다.
        if (CurState != newState || forceReset)
        {
            if (CurState != null)
            {
                PrevState = CurState;
                CurState.Exit();
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

    public void RevertToPrevState()
    {
        ChangeState(PrevState);
    }

    // -------------------------- State 수행 ----------------------------------
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
