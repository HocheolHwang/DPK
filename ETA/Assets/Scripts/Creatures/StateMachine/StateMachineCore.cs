using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State Machine을 사용하는 모든 Agent는 해당 SMCore를 가지거나 상속한다.
/// </summary>
public class StateMachineCore : MonoBehaviour
{
    // Agent의 공통 Component
    [SerializeField] public Animator animator;

    public StateMachine machine;

    
    // 개발 편의성을 위함
    protected void ChangeState(State newState, bool forceReset = false)
    {
        machine.ChangeState(newState, forceReset);
    }

    // Agent의 Behaviour Object에 존재하는 모든 State를 가져와서 Instance화 한다. 
    public void InitInstance()
    {
        // Agent에 SM을 세팅한다.
        machine = new StateMachine();

        // 모든 State는 SMCore를 가진다.
        State[] allChildStates = GetComponentsInChildren<State>();
        foreach (State state in allChildStates)
        {
            state.SetCore(this);
        }
    }
}
