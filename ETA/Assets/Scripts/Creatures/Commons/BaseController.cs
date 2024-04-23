using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseController : MonoBehaviour, IDamageable
{
    protected StateMachine _stateMachine;
    protected State _curState;

    [Header("Common Property")]
    [SerializeField] public Animator animator;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public Detector detector;


    public StateMachine StateMachine { get => _stateMachine; set => _stateMachine = value; }
    public State CurState { get => _curState; set => _curState = _stateMachine.CurState; }


    //-----------------------------------  Essential Functions --------------------------------------------
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        detector = GetComponent<Detector>();
    }
    protected virtual void Update()
    {
        _stateMachine.Execute();
    }
    protected abstract void Init();

    //----------------------------------- State Machine Functions --------------------------------------------
    public void ChangeState(State newState)
    {
        // controller의 curState를 계속 갱신할 수 있다.
        _curState = newState;
        _stateMachine.ChangeState(newState);
    }
    public void RevertToPrevState()
    {
        _curState = _stateMachine.PrevState;
        _stateMachine.RevertToPrevState();
    }

    //----------------------------------- Debugging --------------------------------------------
    private void OnDrawGizmos()
    {

        if (_stateMachine != null && _curState != null)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;

            string label = "Active State: " + _curState.ToString();
            Handles.Label(transform.position, label, style);
        }
    }

    // ---------------------------------- IDamage ------------------------------------------
    public virtual void TakeDamage(int damage)
    {
    }

    public virtual void DestroyEvent()
    {
    }
}
