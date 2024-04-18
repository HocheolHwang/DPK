using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

// start와 update 같은건 각 객체에서 사용
public abstract class BaseController : MonoBehaviour
{
    protected StateMachine _machine;
    protected State _curState;

    [Header("Common Property")]
    [SerializeField] public Animator animator;
    [SerializeField] public NavMeshAgent agent;
    public StateMachine Machine { get => _machine; set => _machine = value; }
    public State CurState { get => _curState; set => _curState = Machine.CurState; }


    protected abstract void Init();

    // 개발 편의성
    public void ChangeState(State newState)
    {
        _curState = newState;           // controller의 curState를 계속 갱신할 수 있다.
        Machine.ChangeState(newState);
    }
    public void RevertToPrevState()
    {
        Machine.RevertToPrevState();
    }
    
    // Debugging STATE
    private void OnDrawGizmos()
    {
        if (Machine != null && _curState != null)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;

            string label = "Active State: " + _curState.ToString();
            Handles.Label(transform.position, label, style);
        }
    }
}
