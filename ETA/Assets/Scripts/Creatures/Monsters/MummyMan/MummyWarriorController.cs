using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MummyWarriorStateItem;
using System;
using Photon.Pun;

public class MummyWarriorController : BaseMonsterController
{
    #region STATE
    public State IDLE_STATE;
    public State IDLE_BATTLE_STATE;
    public State CHASE_STATE;
    public State ATTACK_STATE;
    public State WIND_MILL_STATE;
    public State DIE_STATE;
    public State GLOBAL_STATE;
    #endregion

    #region STATE VARIABLE
    [SerializeField] private float _windMillTime;
    private const float _threadHoldWindMill = 15.0f;

    public float WindMillTime { get => _windMillTime; set => _windMillTime = value; }
    public float ThreadHoldWindMill { get => _threadHoldWindMill; }
    #endregion

    private MummyWarriorAnimationData _animData;
    public MummyWarriorAnimationData AnimData { get => _animData; }
    public Action OnDeath;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    protected override void Start()
    {
        ChangeState(IDLE_STATE);
    }

    // ---------------------------------- Init ------------------------------------------
    protected override void Init()
    {
        _animData = GetComponent<MummyWarriorAnimationData>();
        _animData.StringAnimToHash();

        // ----------------------------- Animation && State -------------------------------------

        _stateMachine = new StateMachine();

        IDLE_STATE = new IdleState(this);
        IDLE_BATTLE_STATE = new IdleBattleState(this);
        CHASE_STATE = new ChaseState(this);
        ATTACK_STATE = new AttackState(this);
        WIND_MILL_STATE = new WindMillState(this);
        DIE_STATE = new DieState(this);
        GLOBAL_STATE = new GlobalState(this);

        _stateMachine.SetGlobalState(GLOBAL_STATE);

        Agent.stoppingDistance = Detector.AttackRange;      // 공격 사거리와 멈추는 거리를 같게 세팅
        UnitType = Define.UnitType.MummyManWarrior;
    }

    // ---------------------------------- IDamage ------------------------------------------
    public override void DestroyEvent()
    {
        // Summon Class에서 세팅한 함수를 수행
        OnDeath?.Invoke();

        base.DestroyEvent();
    }

    public void ChangeToIdleState()
    {
        photonView.RPC("RPC_ChangeToIdleState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToIdleState()
    {
        ChangeState(IDLE_STATE);
    }
    // ---------
    public void ChangeToIdleBattleState()
    {
        photonView.RPC("RPC_ChangeToIdleBattleState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToIdleBattleState()
    {
        ChangeState(IDLE_BATTLE_STATE);
    }


    public void ChangeToChaseState()
    {
        photonView.RPC("RPC_ChangeToChaseState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToChaseState()
    {
        ChangeState(CHASE_STATE);
    }



    public void ChangeToAttackState()
    {
        photonView.RPC("RPC_ChangeToAttackState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToAttackState()
    {
        ChangeState(ATTACK_STATE);
    }


    public void ChangeToWindMillState()
    {
        photonView.RPC("RPC_ChangeToWindMillState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToWindMillState()
    {
        ChangeState(WIND_MILL_STATE);
    }


    public void ChangeToDieState()
    {
        photonView.RPC("RPC_ChangeToDieState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToDieState()
    {
        ChangeState(DIE_STATE);
    }

    [PunRPC]
    void RPC_TakeDamage(int attackDamage, bool isCounter, int shield, bool evasion, int defense)
    {
        CalcDamage(attackDamage, isCounter, shield, evasion, defense);
    }
}
