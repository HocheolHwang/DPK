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
    public State QSKILL_STATE;
    public State COLLAVO_STATE;
    [SerializeField]
    public Transform _destination;


    bool isCollavoration1 = false;
    bool isCollavoration2 = false;


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
        QSKILL_STATE = new QSkillState(this);
        COLLAVO_STATE = new CollavoState(this);

        //_destination = GameObject.Find("FRONT_2").transform;

        ChangeState(IDLE_STATE);
        //ChangeState(MOVE_STATE);
        StartCoroutine(StartMove());
        Managers.Input.KeyAction -= KeyEvent;
        Managers.Input.KeyAction += KeyEvent;

    }

    IEnumerator StartMove()
    {
        yield return new WaitForSeconds(3);
        ChangeState(MOVE_STATE);
    }


    void KeyEvent()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeState(QSKILL_STATE);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            isCollavoration1 = true;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            isCollavoration2 = true;
        }

        if(isCollavoration1 && isCollavoration2)
        {
            ChangeState(COLLAVO_STATE);
            isCollavoration1 = false;
            isCollavoration2 = false;
        }
    }


}
