using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


using PlayerStates;
using Photon.Pun;

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
    public bool isFinished;

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
        //photonView = gameObject.GetComponent<PhotonView>();
        
        _stateMachine.SetGlobalState(GLOBAL_STATE);

        // TODO : 번호에 따른 목적지 변경 해줘야함
        

        //ChangeState(IDLE_STATE);
        
        
        if (photonView.IsMine)
        {
            if(PhotonNetwork.IsMasterClient) _destination = GameObject.Find("FRONT_2").transform;
            else _destination = GameObject.Find("FRONT_3").transform;
            //ChangeState(MOVE_STATE);
            Managers.Input.KeyAction -= KeyEvent;
            Managers.Input.KeyAction += KeyEvent;

            Managers.Input.MouseAction -= MouseEvent;
            Managers.Input.MouseAction += MouseEvent;
            Camera.main.GetComponent<CameraController>()._player = gameObject;
        }
        Debug.Log($"{photonView.IsMine}");


        // 캐릭터가 다 생성 되었기 때문에 신호르 보내줍니다.
        // "내 캐릭터는 완성 되었다"
        FindObjectOfType<GameSystem>().SendCharacherInstantiatedMsg();





    }


    void KeyEvent()
    {
        if (CurState is SkillState || CurState is DieState || CurState is HoldState || CurState is CollavoState) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _usingSkill = Define.SkillKey.Q;
            SyncUsingSkill(Define.SkillKey.Q);
            SkillSlot.SelectSkill(Define.SkillKey.Q);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            _usingSkill = Define.SkillKey.W;
            SyncUsingSkill(Define.SkillKey.W);
            SkillSlot.SelectSkill(Define.SkillKey.W);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _usingSkill = Define.SkillKey.E;
            SyncUsingSkill(Define.SkillKey.E);
            SkillSlot.SelectSkill(Define.SkillKey.E);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _usingSkill = Define.SkillKey.R;
            SyncUsingSkill(Define.SkillKey.R);
            SkillSlot.SelectSkill(Define.SkillKey.R);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _usingSkill = Define.SkillKey.A;
            SyncUsingSkill(Define.SkillKey.A);
            SkillSlot.SelectSkill(Define.SkillKey.A);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _usingSkill = Define.SkillKey.S;
            SyncUsingSkill(Define.SkillKey.S);
            SkillSlot.SelectSkill(Define.SkillKey.S);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _usingSkill = Define.SkillKey.D;
            SyncUsingSkill(Define.SkillKey.D);
            SkillSlot.SelectSkill(Define.SkillKey.D);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            _usingSkill = Define.SkillKey.F;
            SyncUsingSkill(Define.SkillKey.F);
            SkillSlot.SelectSkill(Define.SkillKey.F);
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

    public override void AttackedEvent()
    {
        base.AttackedEvent();
        Managers.Sound.Play("Player/Attacked");
    }

    /// <summary>
    /// 다른 컴포넌트로 옮겨야 할듯
    /// </summary>


    public void SyncUsingSkill(Define.SkillKey key)
    {
        photonView.RPC("RPC_SetUsingSkill", RpcTarget.Others, key);
    }

    public void ChangeToIdleState()
    {
        photonView.RPC("RPC_ChangeToIdleState", RpcTarget.Others);
    }
    public void ChangeToMoveState()
    {
        photonView.RPC("RPC_ChangeToMoveState", RpcTarget.Others);
    }

    public void ChangeToAttackState()
    {
        photonView.RPC("RPC_ChangeToAttackState", RpcTarget.Others);
    }

    public void ChangeToSkillState()
    {
        photonView.RPC("RPC_ChangeToSkillState", RpcTarget.Others);
    }

    public void ChangeToHoldState()
    {
        photonView.RPC("RPC_ChangeToHoldState", RpcTarget.Others);
    }

    public void ChangeToCollavoState()
    {
        photonView.RPC("RPC_ChangeToCollavoState", RpcTarget.Others);
    }

    public void ChangeToDieState()
    {
        photonView.RPC("RPC_ChangeToDieState", RpcTarget.Others);
    }



    [PunRPC]
    void RPC_SetUsingSkill(Define.SkillKey key)
    {
        _usingSkill = key;
    }
    [PunRPC]
    void RPC_ChangeToIdleState()
    {
        ChangeState(IDLE_STATE);
    }

    [PunRPC]
    void RPC_ChangeToAttackState()
    {
        ChangeState(ATTACK_STATE);
    }

    [PunRPC]
    void RPC_ChangeToMoveState()
    {
        ChangeState(MOVE_STATE);
    }

    [PunRPC]
    void RPC_ChangeToSkillState()
    {
        ChangeState(SKILL_STATE);
    }

    [PunRPC]
    void RPC_ChangeToHoldState()
    {
        ChangeState(HOLD_STATE);
    }

    [PunRPC]
    void RPC_ChangeToCollavoState()
    {
        ChangeState(COLLAVO_STATE);
    }
    //RPC_ChangeToDieState
    [PunRPC]
    void RPC_ChangeToDieState()
    {
        ChangeState(DIE_STATE);
    }

    [PunRPC]
    void RPC_TakeDamage(int attackDamage, bool isCounter)
    {
        CalcDamage(attackDamage, isCounter);
    }
}
