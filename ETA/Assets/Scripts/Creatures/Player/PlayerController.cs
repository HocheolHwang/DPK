using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


using PlayerStates;
public class PlayerController : BaseController
{

    public State IDLE_STATE;
    public State MOVE_STATE;
    public State SKILL_STATE;
    [SerializeField]
    public Transform _destination;

    private void Start()
    {
        Init();

    }

    protected override void Init()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        _stateMachine = new StateMachine();

        IDLE_STATE = new IdleState(this);
        MOVE_STATE = new MoveState(this);
        SKILL_STATE = new SkillState(this);

        //_destination = GameObject.Find("FRONT_2").transform;

        ChangeState(IDLE_STATE);
        //ChangeState(MOVE_STATE);
        StartCoroutine(StartMove());

    }

    IEnumerator StartMove()
    {
        yield return new WaitForSeconds(3);
        ChangeState(MOVE_STATE);
    }


}
