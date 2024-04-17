using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// start와 update 같은건 각 객체에서 사용
public class BaseController : MonoBehaviour
{
    [Header("Common Component")]
    [SerializeField] public Animator animator;
    [SerializeField] public NavMeshAgent agent;


    public StateMachine machine;
    public State curState => machine.curState;


    protected virtual void Init() 
    {
        machine = new StateMachine();
    }

    // 개발 편의성
    protected void ChangeState(State newState, bool forceReset = false)
    {
        machine.ChangeState(newState, forceReset);
    }
    
    // Debugging STATE
    private void OnDrawGizmos()
    {
        if (machine != null)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;


            string label = "Active State: " + machine.curState.ToString();
            UnityEditor.Handles.Label(transform.position, label, style);
        }
    }
}
