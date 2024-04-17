using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

// start와 update 같은건 각 객체에서 사용
public abstract class BaseController : MonoBehaviour
{
    [Header("Common Component")]
    [SerializeField] public Animator _animator;
    [SerializeField] public NavMeshAgent _agent;


    public StateMachine _machine;
    public State _curState => _machine._curState;


    protected abstract void Init();

    // 개발 편의성
    protected void ChangeState(State newState, bool forceReset = false)
    {
        _machine.ChangeState(newState, forceReset);
    }
    
    // Debugging STATE
    private void OnDrawGizmos()
    {
        if (_machine != null)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;

            string label = "Active State: " + _machine._curState.ToString();
            Handles.Label(transform.position, label, style);
        }
    }
}
