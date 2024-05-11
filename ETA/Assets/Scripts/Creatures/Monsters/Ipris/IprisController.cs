using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IprisStateItem;

public class IprisController : BaseMonsterController
{
    #region STATE
    public State IDLE_STATE;
    public State IDLE_BATTLE_STATE;
    public State CHASE_STATE;
    public State ATTACK_STATE;
    public State DIE_STATE;
    public State TO_DRAGON_STATE;
    public State GROGGY_STATE;
    public State GLOBAL_STATE;
    public State BUFF_STATE;
    public State COUNTER_ENABLE_STATE;
    public State COUNTER_ATTACK_STATE;
    public State PATTERN_ONE_ENABLE_STATE;
    public State PATTERN_ONE_STATE;
    public State PATTERN_TWO_STATE;
    public State PATTERN_TWO_WINDMILL_STATE;
    #endregion

    #region STATE VARIABLE
    private bool _meetPlayer;
    private float _counterTime;
    private float _threadHoldCounter = 10.0f;

    public bool MeetPlayer { get => _meetPlayer; set => _meetPlayer = value; }
    public float CounterTime { get => _counterTime; set => _counterTime = value; }
    public float ThreadHoldCounter { get => _threadHoldCounter; set => _threadHoldCounter = value; }
    #endregion

    private IprisAnimationData _animData;
    public IprisAnimationData AnimData { get => _animData; }

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
        _animData = GetComponent<IprisAnimationData>();
        _animData.StringAnimToHash();
        _stateMachine = new StateMachine();

        IDLE_STATE = new IdleState(this);
        IDLE_BATTLE_STATE = new IdleBattleState(this);
        CHASE_STATE = new ChaseState(this);
        ATTACK_STATE = new AttackState(this);
        GROGGY_STATE = new GroggyState(this);
        DIE_STATE = new DieState(this);
        GLOBAL_STATE = new GlobalState(this);
        _stateMachine.SetGlobalState(GLOBAL_STATE);

        COUNTER_ENABLE_STATE = new CounterEnable(this);
        COUNTER_ATTACK_STATE = new CounterAttack(this);



        Agent.stoppingDistance = Detector.AttackRange - 0.3f;
        UnitType = Define.UnitType.Ipris;
    }
}
