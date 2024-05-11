using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MummyManStateItem;
using Photon.Pun;

public class MummyManController : BaseMonsterController
{
    [Header("RightHand")]
    [SerializeField] public GameObject StoneSpawned;

    #region STATE
    public State IDLE_STATE;
    public State IDLE_BATTLE_STATE;
    public State CHASE_STATE;
    public State ATTACK_STATE;
    public State THROW_STATE;
    public State CLAP_STATE;
    public State SHOUTING_STATE;
    public State JUMP_STATE;
    public State BACK_LOCATION_STATE;
    public State FORE_SHADOWING_STATE;
    public State RUSH_STATE;
    public State WIND_MILL_STATE;
    public State DIE_STATE;
    public State GROGGY_STATE;
    public State GLOBAL_STATE;
    #endregion

    #region STATE VARIABLE
    [Header("STATE VARIABLE")]
    [SerializeField] private bool _meetPlayer;                     // 플레이어와 첫 조우 여부
    [SerializeField] private bool _isRangedAttack = true;          // 원거리 디텍터를 활성화한 상태
    [SerializeField] private bool _isRush;                         // Rush Pattern 수행
    [SerializeField] private float _shoutingTime;
    [SerializeField] private float _threadHoldShouting = 14.0f;    // 14초
    [SerializeField] private float _jumpTime;
    [SerializeField] private float _threadHoldJump = 30.5f;        // 30.5초
    [SerializeField] private Transform _target;
    [SerializeField] private float _attackRange;
    [SerializeField] private Vector3 _startPos;
    [SerializeField] private Vector3 _destPos;

    public bool MeetPlayer { get => _meetPlayer; set => _meetPlayer = value; }
    public bool IsRangedAttack { get => _isRangedAttack; set => _isRangedAttack = value; }
    public bool IsRush { get => _isRush; set => _isRush = value; }
    public float ShoutingTime { get => _shoutingTime; set => _shoutingTime = value; }
    public float ThreadHoldShouting { get => _threadHoldShouting; set => _threadHoldShouting = value; }
    public float JumpTime { get => _jumpTime; set => _jumpTime = value; }
    public float ThreadHoldJump { get => _threadHoldJump; set => _threadHoldJump = value; }
    public Transform Target { get => _target; set => _target = value; }
    public float AttackRange { get => _attackRange; set => _attackRange = value; }
    public Vector3 StartPos { get => _startPos; set => _startPos = value; }
    public Vector3 DestPos { get => _destPos; set => _destPos = value; }
    #endregion

    private MummyManAnimationData _animData;
    private SummonSkill _summonSkill;
    public MummyManAnimationData AnimData { get => _animData; }
    public SummonSkill SummonSkill { get => _summonSkill; }

    public Action OnBossDestroyed;

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
        _animData = GetComponent<MummyManAnimationData>();
        _animData.StringAnimToHash();
        _summonSkill = GetComponent<SummonSkill>();

        // ----------------------------- Animation && State -------------------------------------

        _stateMachine = new StateMachine();

        // BASE
        IDLE_STATE = new IdleState(this);
        IDLE_BATTLE_STATE = new IdleBattleState(this);
        CHASE_STATE = new ChaseState(this);
        ATTACK_STATE = new AttackState(this);
        // SOMMON
        CLAP_STATE = new ClapState(this);

        SHOUTING_STATE = new ShoutingState(this);
        JUMP_STATE = new JumpState(this);
        BACK_LOCATION_STATE = new BackLocationState(this);
        RUSH_STATE = new RushState(this);
        WIND_MILL_STATE = new WindMillState(this);
        FORE_SHADOWING_STATE = new ForeShadowingState(this);

        // Global
        DIE_STATE = new DieState(this);
        GROGGY_STATE = new GroggyState(this);
        GLOBAL_STATE = new GlobalState(this);

        _stateMachine.SetGlobalState(GLOBAL_STATE);
        // 공격 사거리와 멈추는 거리를 같게 세팅 -> StateItem의 SetDetector에서 세팅
        UnitType = Define.UnitType.MummyMan;

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


    public void ChangeToClapState()
    {
        photonView.RPC("RPC_ChangeToClapState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToClapState()
    {
        ChangeState(CLAP_STATE);
    }

    public void ChangeToShoutingState()
    {
        photonView.RPC("RPC_ChangeToShoutingState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToShoutingState()
    {
        ChangeState(SHOUTING_STATE);
    }
    public void ChangeToJumpState()
    {
        photonView.RPC("RPC_ChangeToJumpState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToJumpState()
    {
        ChangeState(JUMP_STATE);
    }


    public void ChangeToRushState()
    {
        photonView.RPC("RPC_ChangeToRushState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToRushState()
    {
        ChangeState(RUSH_STATE);
    }



    public void ChangeToWindMillState()
    {
        photonView.RPC("RPC_ChangeToWindMillState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToWindMillState()
    {
        Debug.Log("Wind Mill RPC 받았따");
        ChangeState(WIND_MILL_STATE);
    }


    public void ChangeToForeShadowingState()
    {
        photonView.RPC("RPC_ChangeToForeShadowingState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToForeShadowingState()
    {
        ChangeState(FORE_SHADOWING_STATE);
    }


    public void ChangeToBackLocationState()
    {
        photonView.RPC("RPC_ChangeToBackLocationState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToBackLocationState()
    {
        ChangeState(BACK_LOCATION_STATE);
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
    void RPC_TakeDamage(int attackDamage, bool isCounter, int shield, bool evasion, int defense)
    {
        CalcDamage(attackDamage, isCounter, shield, evasion, defense);
    }
}
