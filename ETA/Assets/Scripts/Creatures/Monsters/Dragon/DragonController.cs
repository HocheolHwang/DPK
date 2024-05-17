
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DragonStateItem;
using System;
using Unity.VisualScripting;
using Photon.Pun;

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
    public State CRY_TO_FIRE_STATE;
    public State GROUND_TO_SKY_STATE;
    public State SKY_DOWN_ATTACK_STATE;
    public State FLY_FIRE_BALL_STATE;
    #endregion

    #region STATE VARIABLE
    [Header("STATE VARIABLE")]
    [SerializeField] private bool _meetPlayer;          // 플레이어 만남 여부
    [SerializeField] private bool _isCryToDown;         // CryToDown 상태 여부( Ground To Sky 상태에서 사용 )
    [SerializeField] private bool _isBreath;            // Breath 상태 수행했으면 true
    [SerializeField] private bool _isFireball;          // Fireball 상태 수행했으면 true
    [SerializeField] private bool _isMeetConditionHit;  // hitAttackCnt가 ThreadHoldHitAttackCnt를 만족
    [SerializeField] private bool _isMeetConditionDown; // _hitCounterCnt가 ThreadHoldCryDown을 만족
    [SerializeField] private bool _isMeetConditionFire; // _hitCounterCnt가 ThreadHoldCryFireball을 만족

    [SerializeField] private float _attackCnt;          // 일반 공격 횟수
    [SerializeField] private float _fearTime;           // Fear 패턴 주기
    [SerializeField] private float _cryDownTime;        // CryDown 패턴 주기
    [SerializeField] private int _hitAttackCnt;         // 피격 횟수


    public bool MeetPlayer { get => _meetPlayer; set => _meetPlayer = value; }
    public bool IsCryToDown { get => _isCryToDown; set => _isCryToDown = value; }
    public bool IsBreath { get => _isBreath; set => _isBreath = value; }
    public bool IsFireball { get => _isFireball; set => _isFireball = value; }
    public bool IsMeetConditionHit { get => _isMeetConditionHit; set => _isMeetConditionHit = value; }
    public bool IsMeetConditionDown { get => _isMeetConditionDown; set => _isMeetConditionDown = value; }
    public bool IsMeetConditionFire { get => _isMeetConditionFire; set => _isMeetConditionFire = value; }
    public float AttackCnt { get => _attackCnt; set => _attackCnt = value; }
    public int ChangeAttackCount { get => 3; }
    public float FearTime { get => _fearTime; set => _fearTime = value; }
    public float ThreadHoldFearTime { get => 15.0f; }
    public float CryDownTime { get => _cryDownTime; set => _cryDownTime = value; }
    public float ThreadHoldCryTime { get => 30.0f; }
    public int HitAttackCnt { get => _hitAttackCnt; set => _hitAttackCnt = value; }
    public int ThreadHoldHitAttackCnt { get => 20; }

    public int AmountDEF { get => 1000; }
    #endregion

    #region COUNTER INFO
    [Header("COUNTER INFO")]
    [SerializeField] private int _hitCounterCnt;

    public int HitCounterCnt { get => _hitCounterCnt; set => _hitCounterCnt = value; }
    public int ThreadHoldCryDown { get => 2; }
    public int ThreadHoldCryFireball { get => 3; }
    #endregion

    [Header("Tail Effect")]
    [SerializeField] public ParticleSystem TailEffect;
    [SerializeField] public ParticleSystem FearEnableEffect;
    [SerializeField] public ParticleSystem BreathReadyEffect;
    [SerializeField] public ParticleSystem BreathEffect;

    public static event Action OnBossDestroyed;

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

        BREATH_ENABLE_STATE = new BreathEnableState(this);
        BREATH_STATE = new BreathState(this);
        #endregion

        #region SKY PATTERN STATE
        CRY_TO_DOWN_STATE = new CryToDownState(this);
        GROUND_TO_SKY_STATE = new GroundToSkyState(this);
        SKY_DOWN_ATTACK_STATE = new SkyDownAttackState(this);

        CRY_TO_FIRE_STATE = new CryToFireState(this);
        FLY_FIRE_BALL_STATE = new FlyFireballState(this);
        // SKY TO GROUND
        #endregion

        Agent.stoppingDistance = Detector.AttackRange - 0.3f;
        // 넉백 히트박스를 몸에 가지고 다니자.
        UnitType = Define.UnitType.Dragon;
        _destroyedTime = _animData.DieAnim.length;
    }

    // ---------------------------- Breath Enable ----------------------------------
    public override void AttackedEvent()
    {
        if (CurState == BREATH_ENABLE_STATE)
        {
            _hitAttackCnt++;
            if (_hitAttackCnt >= ThreadHoldHitAttackCnt)
            {
                _isMeetConditionHit = true;
            }
        }
        else
        {
            _hitAttackCnt = 0;
            _isMeetConditionHit = false;
        }
    }

    // ---------------------------- Multi Counter ----------------------------------
    public override void CounterEvent()
    {
        base.CounterEvent();

        if (CurState == CRY_TO_DOWN_STATE)
        {
            _hitCounterCnt++;
            if (_hitCounterCnt >= ThreadHoldCryDown)
            {
                _isMeetConditionDown = true;
            }
        }
        else if (CurState == CRY_TO_FIRE_STATE)
        {
            _hitCounterCnt++;
            if (_hitCounterCnt >= ThreadHoldCryDown)
            {
                _isMeetConditionDown = true;
            }

            if (_hitCounterCnt >= ThreadHoldCryFireball)
            {
                _isMeetConditionFire = true;
            }
        }
    }
    public override void DestroyEvent()
    {
        // 보스몬스터 죽었을때 이벤트 발생
        OnBossDestroyed?.Invoke();

        base.DestroyEvent();
    }

    /// 
    /// @@@@@@@@@@@@@@@@@

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
    public void ChangeToCryState()
    {
        photonView.RPC("RPC_ChangeToCryState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToCryState()
    {
        ChangeState(CRY_STATE);
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

    public void ChangeToFearEnableState()
    {
        photonView.RPC("RPC_ChangeToFearEnableState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToFearEnableState()
    {
        ChangeState(FEAR_ENABLE_STATE);
    }

    public void ChangeToFearAttackState()
    {
        photonView.RPC("RPC_ChangeToFearAttackState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToFearAttackState()
    {
        ChangeState(FEAR_ATTACK_STATE);
    }

    public void ChangeToFearStrongAttackState()
    {
        photonView.RPC("RPC_ChangeToFearStrongAttackState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToFearStrongAttackState()
    {
        ChangeState(FEAR_STRONG_ATTACK_STATE);
    }

    public void ChangeToBreathEnableState()
    {
        photonView.RPC("RPC_ChangeToBreathEnableState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToBreathEnableState()
    {
        ChangeState(BREATH_ENABLE_STATE);
    }

    public void ChangeToBreathState()
    {
        photonView.RPC("RPC_ChangeToBreathState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToBreathState()
    {
        ChangeState(BREATH_STATE);
    }

    public void ChangeToCryToDownState()
    {
        photonView.RPC("RPC_ChangeToCryToDownState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToCryToDownState()
    {
        ChangeState(CRY_TO_DOWN_STATE);
    }

    public void ChangeToGroundToSkyState()
    {
        photonView.RPC("RPC_ChangeToGroundToSkyState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToGroundToSkyState()
    {
        ChangeState(GROUND_TO_SKY_STATE);
    }

    public void ChangeToSkyDownAttackState()
    {
        photonView.RPC("RPC_ChangeToSkyDownAttackState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToSkyDownAttackState()
    {
        ChangeState(SKY_DOWN_ATTACK_STATE);
    }

    public void ChangeToCryToFireState()
    {
        photonView.RPC("RPC_ChangeToCryToFireState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToCryToFireState()
    {
        ChangeState(CRY_TO_FIRE_STATE);
    }

    public void ChangeToFlyFireBallState()
    {
        photonView.RPC("RPC_ChangeToFlyFireBallState", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_ChangeToFlyFireBallState()
    {
        ChangeState(FLY_FIRE_BALL_STATE);
    }


    [PunRPC]
    void RPC_TakeDamage(int attackDamage, bool isCounter, int shield, bool evasion, int defense)
    {
        CalcDamage(attackDamage, isCounter, shield, evasion, defense);
    }

}
