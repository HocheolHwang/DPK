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
    [Header("STATE VARIABLE")]
    [SerializeField] private bool _meetPlayer;
    [SerializeField] private float _counterTime;
    private const float _threadHoldCounter = 11.0f;       // 11초
    [SerializeField] private float _buffTime;
    private const float _threadHoldBuff = 30.0f;          // 30초
    [SerializeField] private float _patternTwoTime;       
    private const float _threadHoldPatternTwo = 20.0f;    // 20초
    [SerializeField] private float _windMillCnt;
    [SerializeField] private int _patternOneCnt;          // HP가 MAX_HP의 절반
    private int _threadHoldPatternOne = 1;                // 1번만 수행
    private bool _isCounterTrigger;                       // Counter 스킬에 맞았는지

    public bool MeetPlayer { get => _meetPlayer; set => _meetPlayer = value; }
    public float CounterTime { get => _counterTime; set => _counterTime = value; }
    public float ThreadHoldCounter { get => _threadHoldCounter; }
    public float BuffTime { get => _buffTime; set => _buffTime = value; }
    public float ThreadHoldBuff { get => _threadHoldBuff; }
    public float PatternTwoTime { get => _patternTwoTime; set => _patternTwoTime = value; }
    public float ThreadHoldPatternTwo { get => _threadHoldPatternTwo; }
    public float WindMillCnt { get => _windMillCnt; set => _windMillCnt = value; }
    public float ThreadHoldWindMill { get => 3; }           // 3번
    public int PatternOneCnt { get => _patternOneCnt; set => _patternOneCnt = value; }
    public int ThreadHoldPatternOne { get => _threadHoldPatternOne; }
    public bool IsCounterTrigger { get => _isCounterTrigger; set => _isCounterTrigger = value; }
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

        COUNTER_ENABLE_STATE = new CounterEnableState(this);
        COUNTER_ATTACK_STATE = new CounterAttackState(this);
        BUFF_STATE = new BuffState(this);
        PATTERN_TWO_STATE = new PatternTwoState(this);
        PATTERN_TWO_WINDMILL_STATE = new PatternTwoWindMillState(this);
        PATTERN_ONE_ENABLE_STATE = new PatternOneEnableState(this);
        PATTERN_ONE_STATE = new PatternOneState(this);
        TO_DRAGON_STATE = new ToDragonState(this);


        Agent.stoppingDistance = Detector.AttackRange - 0.3f;
        UnitType = Define.UnitType.Ipris;
    }
}
