using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KnightGStateItem;
using Photon.Pun;

public class KnightGController : BaseMonsterController
{
    // KnightG가 가지는 상태
    public State IDLE_STATE;
    public State IDLE_BATTLE_STATE;
    public State CHASE_STATE;
    public State ATTACK_STATE;
    public State TWO_SKILL_TRANSITION_STATE;
    public State TWO_SKILL_ENERGY_STATE;
    public State TWO_SKILL_ATTACK_STATE;
    public State COUNTER_ENABLE_STATE;
    public State COUNTER_ATTACK_STATE;
    public State PHASE_TRANSITION_STATE;
    public State PHASE_ATTACK_ING_STATE;
    public State DIE_STATE;
    public State GROGGY_STATE;
    public State GLOBAL_STATE;

    private KnightGAnimationData _animData;
    private bool _isEnterPhaseTwo;                 // Phase 진입 여부
    public KnightGAnimationData KnightGAnimData { get => _animData; }
    public bool IsEnterPhaseTwo { get => _isEnterPhaseTwo; set => _isEnterPhaseTwo = value; }

    public static event Action OnBossDestroyed;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    protected override void Start()
    {
        base.Start();
        ChangeState(IDLE_STATE);
    }

    // ---------------------------------- Init ------------------------------------------
    protected override void Init()
    {
        _animData = GetComponent<KnightGAnimationData>();
        _animData.StringAnimToHash();

        // ----------------------------- Animation && State -------------------------------------

        _stateMachine = new StateMachine();

        IDLE_STATE = new IdleState(this);
        IDLE_BATTLE_STATE = new IdleBattleState(this);
        CHASE_STATE = new ChaseState(this);
        ATTACK_STATE = new AttackState(this);
        // Skill
        TWO_SKILL_TRANSITION_STATE = new TwoSkillTransitionState(this);
        TWO_SKILL_ENERGY_STATE = new TwoSkillEnergyState(this);
        TWO_SKILL_ATTACK_STATE = new TwoSkillAttackState(this);
        // Counter
        COUNTER_ENABLE_STATE = new CounterEnableState(this);
        COUNTER_ATTACK_STATE = new CounterAttackState(this);
        // Phase
        PHASE_TRANSITION_STATE = new PhaseTransitionState(this);
        PHASE_ATTACK_ING_STATE = new PhaseAttackingState(this);
        // Global
        DIE_STATE = new DieState(this);
        GROGGY_STATE = new GroggyState(this);
        GLOBAL_STATE = new GlobalState(this);

        _stateMachine.SetGlobalState(GLOBAL_STATE);

        Agent.stoppingDistance = Detector.AttackRange;      // 공격 사거리와 멈추는 거리를 같게 세팅
        UnitType = Define.UnitType.KnightG;
    }

    // ---------------------------------- IDamage ------------------------------------------
    public override void DestroyEvent()
    {
        // 보스몬스터 죽었을때 이벤트 발생
        OnBossDestroyed?.Invoke();

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

    public void ChangeToDieState()
    {
        photonView.RPC("RPC_ChangeToDieState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToDieState()
    {
        ChangeState(DIE_STATE);
    }

    public void ChangeToTwoSkillTransitionState()
    {
        photonView.RPC("RPC_ChangeToTwoSkillTransitionState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToTwoSkillTransitionState()
    {
        ChangeState(TWO_SKILL_TRANSITION_STATE);
    }

    public void ChangeToTwoSkillEnergyState()
    {
        photonView.RPC("RPC_ChangeToTwoSkillEnergyState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToTwoSkillEnergyState()
    {
        ChangeState(TWO_SKILL_ENERGY_STATE);
    }

    public void ChangeToTwoSkillAttackState()
    {
        photonView.RPC("RPC_ChangeToTwoSkillAttackState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToTwoSkillAttackState()
    {
        ChangeState(TWO_SKILL_ATTACK_STATE);
    }

    public void ChangeToCounterEnableState()
    {
        photonView.RPC("RPC_ChangeToCounterEnableState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToCounterEnableState()
    {
        ChangeState(COUNTER_ENABLE_STATE);
    }

    public void ChangeToCounterAttackState()
    {
        photonView.RPC("RPC_ChangeToCounterAttackState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToCounterAttackState()
    {
        ChangeState(COUNTER_ATTACK_STATE);
    }

    public void ChangeToPhaseTransitionState()
    {
        photonView.RPC("RPC_ChangeToPhaseTransitionState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToPhaseTransitionState()
    {
        ChangeState(PHASE_TRANSITION_STATE);
    }

    public void ChangeToPhaseAttackIngState()
    {
        photonView.RPC("RPC_ChangeToPhaseAttackIngState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToPhaseAttackIngState()
    {
        ChangeState(PHASE_ATTACK_ING_STATE, true);
    }

    public void ChangeToGroggyState()
    {
        photonView.RPC("RPC_ChangeToGroggyState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToGroggyState()
    {
        ChangeState(GROGGY_STATE);
    }


    [PunRPC]
    void RPC_TakeDamage(int attackDamage, bool isCounter)
    {
        CalcDamage(attackDamage, isCounter);
    }
}
