using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// State Machine을 사용하는 모든 Agent는 해당 SMCore를 가지거나 상속한다.
/// 모든 Agent는 SM을 가진다.
/// Agent가 State에서 사용하는 모든 Component를 가진다.
/// Agent가 State에서 사용하는 모든 GameObject를 가진다.
/// </summary>
public class StateMachineCore : MonoBehaviour
{
    [Header("Common Component")]
    [SerializeField] public Animator animator;
    [SerializeField] public NavMeshAgent agent;

    //[Header("Common GameObject : MonoBehaviour")]
    //[SerializeField] public DetectTarget deTectTarget;

    public StateMachine machine;    // 하나의 Agent에 하나의 machine만 존재하기 때문에 singleton으로 선언할까?
    public Detector detector;

    /// <summary>
    /// Agent의 Behaviour Object에 존재하는 모든 State를 가져와서 Instance화 한다. 
    /// </summary>
    public void InitInstance()
    {
        agent.stoppingDistance = detector.attackRange;

        // Agent에 SM을 세팅한다.
        machine = new StateMachine();

        // 모든 State는 SMCore를 가진다.
        State[] allChildStates = GetComponentsInChildren<State>();
        foreach (State state in allChildStates)
        {
            Debug.Log($"{state.gameObject.name} SetCore");
            state.SetCore(this);
        }
    }

    /// <summary>
    /// agent가 도착했는지 판단
    /// </summary>
    public bool IsArriveAgent()
    {
        if (detector.target == null) return true;
        //Debug.Log($"IsArriveAgent: {agent.remainingDistance}, {agent.stoppingDistance}, {agent.remainingDistance <= agent.stoppingDistance}");
        return agent.remainingDistance <= agent.stoppingDistance;
    }

    /// <summary>
    /// [ machine의 ChangeState를 호출 ]
    /// 개발 편의성
    /// </summary>
    protected void ChangeState(State newState, bool forceReset = false)
    {
        machine.ChangeState(newState, forceReset);
    }

    /// <summary>
    /// Debugging
    /// </summary>
    private void OnDrawGizmos()
    {
        if (machine != null)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;


            string label = "Active State: " + machine.curState.name;
            UnityEditor.Handles.Label(transform.position, label, style);
        }
    }
}
