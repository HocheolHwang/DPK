using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

// start와 update 같은건 각 객체에서 사용
public abstract class BaseController : MonoBehaviour
{
    [Header("Common Component")]
    [SerializeField] public Animator animator;
    [SerializeField] public NavMeshAgent agent;

    
    public StateMachine Machine { get; set; }
    public State CurState { get => CurState; set => CurState = Machine.CurState; }


    protected abstract void Init();

    // 개발 편의성
    protected void ChangeState(State newState)
    {
        Machine.ChangeState(newState);
    }
    
    // Debugging STATE
    private void OnDrawGizmos()
    {
        if (Machine != null)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;

            string label = "Active State: " + CurState.ToString();
            Handles.Label(transform.position, label, style);
        }
    }
}
