using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


using PlayerStates;
public class PlayerController : BaseController
{

    public State IDLE_STATE;
    public State MOVE_STATE;
    public State ATTACK_STATE;
    public State SKILL_STATE;
    public State COLLAVO_STATE;
    public State DIE_STATE;
    public State HOLD_STATE;
    public State GLOBAL_STATE;
    [SerializeField]
    public Transform _destination;
    public Define.SkillKey _usingSkill;


    public SkillSlot SkillSlot { get; set;}



    private void Start()
    {
        Init();

    }

    protected override void Init()
    {
        _stateMachine = new StateMachine();

        IDLE_STATE = new IdleState(this);
        MOVE_STATE = new MoveState(this);
        ATTACK_STATE = new AttackState(this);
        SKILL_STATE = new SkillState(this);
        COLLAVO_STATE = new CollavoState(this);
        DIE_STATE = new DieState(this);
        HOLD_STATE = new HoldState(this);
        GLOBAL_STATE = new GlobalState(this);

        SkillSlot = gameObject.GetOrAddComponent<SkillSlot>();
        

        _stateMachine.SetGlobalState(GLOBAL_STATE);

        //_destination = GameObject.Find("FRONT_2").transform;

        ChangeState(IDLE_STATE);
        ChangeState(MOVE_STATE);
        //StartCoroutine(StartMove());
        Managers.Input.KeyAction -= KeyEvent;
        Managers.Input.KeyAction += KeyEvent;

        Managers.Input.MouseAction -= MouseEvent;
        Managers.Input.MouseAction += MouseEvent;




    }

    IEnumerator StartMove()
    {
        yield return new WaitForSeconds(3);
        ChangeState(MOVE_STATE);
    }


    void KeyEvent()
    {
        if (CurState is SkillState || CurState is DieState || CurState is HoldState || CurState is CollavoState) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _usingSkill = Define.SkillKey.Q;
            SkillSlot.SelectSkill(Define.SkillKey.Q);
            //ChangeState(SKILL_STATE);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            _usingSkill = Define.SkillKey.W;
            SkillSlot.SelectSkill(Define.SkillKey.W);
            //ChangeState(SKILL_STATE);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _usingSkill = Define.SkillKey.E;
            SkillSlot.SelectSkill(Define.SkillKey.E);
            //ChangeState(SKILL_STATE);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _usingSkill = Define.SkillKey.R;
            //ChangeState(SKILL_STATE);
        }

    }

    void MouseEvent(Define.MouseEvent evt)
    {
        if (Input.GetMouseButtonDown(1))
        {
            SkillSlot.CancleSkill();
        }
    }

    public override void DestroyEvent()
    {
        base.DestroyEvent();
        SkillSlot.Clear();
    }




}
