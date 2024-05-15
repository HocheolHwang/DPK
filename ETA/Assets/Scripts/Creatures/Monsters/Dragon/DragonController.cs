using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DragonStateItem;
using System;

public class DragonController : BaseMonsterController
{
    #region STATE
    public State GROGGY_STATE;
    public State DIE_STATE;
    public State IDLE_STATE;
    public State IDLE_BATTLE_STATE;
    public State CHASE_STATE;
    public State ATTACK_STATE;
    public State CRY_STATE;
    public State GLOBAL_STATE;

    public State FEAR_ENABLE_STATE;
    public State FEAR_ATTACK_STATE;
    public State FEAR_STRONG_ATTACK_STATE;
    public State BREATH_ENABLE_STATE;
    public State BREATH_STATE;
    public State CRY_TO_DOWN_STATE;
    public State CRY_TO_BREATH_STATE;
    public State GROUND_TO_SKY_STATE;
    public State SKY_DOWN_ATTACK_STATE;
    public State FLY_FIRE_BALL_STATE;
    #endregion

    #region STATE VARIABLE
    [Header("STATE VARIABLE")]
    [SerializeField] private bool _meetPlayer;
    [SerializeField] private bool _isCryToSky;

    [SerializeField] private float _attackCnt;
    [SerializeField] private float _fearTime;
    [SerializeField] private float _cryTime;

    public bool MeetPlayer { get => _meetPlayer; set => _meetPlayer = value; }
    public bool IsCryToSky { get => _isCryToSky; set => _isCryToSky = value; }
    public float AttackCnt { get => _attackCnt; set => _attackCnt = value; }
    public int ChangeAttackCount { get => 3; }
    public float FearTime { get => _fearTime; set => _fearTime = value; }
    public float ThreadHoldFearTime { get => 15.0f; }
    public float CryTime { get => _cryTime; set => _cryTime = value; }
    public float ThreadHoldCryTime { get => 30.0f; }

    public int AmountDEF { get => 1000; }
    #endregion

    #region COUNTER INFO
    [Header("COUNTER INFO")]
    [SerializeField] private int _hitCounterCnt;

    public int HitCounterCnt { get => _hitCounterCnt; set => _hitCounterCnt = value; }
    public int ThreadHoldCryDown { get => 2; }
    #endregion

    private DragonAnimationData _animData;
    public DragonAnimationData AnimData { get => _animData; }

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

    protected override void Init()
    {
        _animData = GetComponent<DragonAnimationData>();
        _animData.StringAnimToHash();
        _stateMachine = new StateMachine();

        #region BASIC STATE
        IDLE_STATE = new IdleState(this);
        CHASE_STATE = new ChaseState(this);
        IDLE_BATTLE_STATE = new IdleBattleState(this);
        ATTACK_STATE = new AttackState(this);
        CRY_STATE = new CryState(this);
        DIE_STATE = new DieState(this);
        GROGGY_STATE = new GroggyState(this);
        GLOBAL_STATE = new GlobalState(this);
        _stateMachine.SetGlobalState(GLOBAL_STATE);
        #endregion

        #region GROUND PATTERN STATE
        FEAR_ENABLE_STATE = new FearEnableState(this);
        FEAR_ATTACK_STATE = new FearAttackState(this);
        FEAR_STRONG_ATTACK_STATE = new FearStrongAttackState(this);
        #endregion

        #region SKY PATTERN STATE
        CRY_TO_DOWN_STATE = new CryToDownState(this);
        GROUND_TO_SKY_STATE = new GroundToSkyState(this);
        SKY_DOWN_ATTACK_STATE = new SkyDownAttackState(this);
        #endregion

        Agent.stoppingDistance = Detector.AttackRange - 0.3f;
        // 넉백 히트박스를 몸에 가지고 다니자.
        UnitType = Define.UnitType.Dragon;
        _destroyedTime = _animData.DieAnim.length;
    }
}
