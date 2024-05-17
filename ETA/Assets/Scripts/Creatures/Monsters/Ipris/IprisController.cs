using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IprisStateItem;
using Photon.Pun;

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
    public State PATTERN_ONE_STRONG_STATE;
    public State PATTERN_TWO_STATE;
    public State PATTERN_TWO_WINDMILL_STATE;
    public State BACK_POSITION_STATE;
    #endregion

    #region STATE VARIABLE
    [Header("STATE VARIABLE")]
    [SerializeField] private bool _meetPlayer;
    [SerializeField] private float _counterTime;
    private const float _threadHoldCounter = 11.0f;       // 11초
    [SerializeField] private float _buffTime;
    private const float _threadHoldBuff = 15.0f;          // 15초
    [SerializeField] private float _patternTwoTime;       
    private const float _threadHoldPatternTwo = 21.0f;    // 21초
    [SerializeField] private float _windMillCnt;
    [SerializeField] private int _patternOneCnt;          // HP가 MAX_HP의 절반
    private int _threadHoldPatternOne = 1;                // 1번만 수행
    [SerializeField] private Vector3 _startPos;
    [SerializeField] private Vector3 _destPos;

    public bool MeetPlayer { get => _meetPlayer; set => _meetPlayer = value; }
    public float CounterTime { get => _counterTime; set => _counterTime = value; }
    public float ThreadHoldCounter { get => _threadHoldCounter; }
    public float BuffTime { get => _buffTime; set => _buffTime = value; }
    public float ThreadHoldBuff { get => _threadHoldBuff; }
    public float PatternTwoTime { get => _patternTwoTime; set => _patternTwoTime = value; }
    public float ThreadHoldPatternTwo { get => _threadHoldPatternTwo; }
    public float WindMillCnt { get => _windMillCnt; set => _windMillCnt = value; }
    public float ThreadHoldWindMill { get => 2; }           // 3번
    public int PatternOneCnt { get => _patternOneCnt; set => _patternOneCnt = value; }
    public int ThreadHoldPatternOne { get => _threadHoldPatternOne; }
    public Vector3 StartPos { get => _startPos; set => _startPos = value; }
    public Vector3 DestPos { get => _destPos; set => _destPos = value; }
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
        ChangeState(IDLE_STATE);

        COUNTER_ENABLE_STATE = new CounterEnableState(this);
        COUNTER_ATTACK_STATE = new CounterAttackState(this);
        BUFF_STATE = new BuffState(this);
        PATTERN_TWO_STATE = new PatternTwoState(this);
        PATTERN_TWO_WINDMILL_STATE = new PatternTwoWindMillState(this);
        BACK_POSITION_STATE = new BackPositionState(this);
        PATTERN_ONE_ENABLE_STATE = new PatternOneEnableState(this);
        PATTERN_ONE_STATE = new PatternOneState(this);
        PATTERN_ONE_STRONG_STATE = new PatternOneStrongState(this);
        TO_DRAGON_STATE = new ToDragonState(this);


        Agent.stoppingDistance = Detector.AttackRange - 0.3f;
        UnitType = Define.UnitType.Ipris;

        _counterTime = 4.0f;
        _buffTime = 10.0f;
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

    public void ChangeToGroggyState()
    {
        photonView.RPC("RPC_ChangeToGroggyState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToGroggyState()
    {
        ChangeState(GROGGY_STATE);
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
    public void ChangeToBuffState()
    {
        photonView.RPC("RPC_ChangeToBuffState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToBuffState()
    {
        ChangeState(BUFF_STATE);
    }


    public void ChangeToPatternOneEnableState()
    {
        photonView.RPC("RPC_ChangeToPatternOneEnableState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToPatternOneEnableState()
    {
        ChangeState(PATTERN_ONE_ENABLE_STATE);
    }
    public void ChangeToPatternOneState()
    {
        photonView.RPC("RPC_ChangeToPatternOneState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToPatternOneState()
    {
        ChangeState(PATTERN_ONE_STATE);
    }
    public void ChangeToPatternOneStrongState()
    {
        photonView.RPC("RPC_ChangeToPatternOneStrongState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToPatternOneStrongState()
    {
        ChangeState(PATTERN_ONE_STRONG_STATE);
    }

    public void ChangeToPatternTwoState()
    {
        photonView.RPC("RPC_ChangeToPatternTwoState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToPatternTwoState()
    {
        ChangeState(PATTERN_TWO_STATE);
    }
    public void ChangeToPatternTwoWindmillState()
    {
        photonView.RPC("RPC_ChangeToPatternTwoWindmillState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToPatternTwoWindmillState()
    {
        ChangeState(PATTERN_TWO_WINDMILL_STATE);
    }
    public void ChangeToBackPositionState()
    {
        photonView.RPC("RPC_ChangeToBackPositionState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToBackPositionState()
    {
        ChangeState(BACK_POSITION_STATE);
    }
    public void ChangeToToDrangonState()
    {
        photonView.RPC("RPC_ChangeToToDrangonState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToToDrangonState()
    {
        ChangeState(TO_DRAGON_STATE);
    }




    [PunRPC]
    void RPC_TakeDamage(int attackDamage, bool isCounter, int shield, bool evasion, int defense)
    {
        CalcDamage(attackDamage, isCounter, shield, evasion, defense);
    }
}
