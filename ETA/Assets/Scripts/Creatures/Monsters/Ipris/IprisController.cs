using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IprisController : BaseMonsterController
{
    public State IDLE_STATE;
    public State IDLE_BATTLE_STATE;
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
        _stateMachine.SetGlobalState(GLOBAL_STATE);



        // StopDistance 세팅?
        UnitType = Define.UnitType.Ipris;
    }
}
