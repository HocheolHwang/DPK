using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class NormalMonsterController : BaseController
{
    // Normal Monster Controller만 가지는 상태
    public State IDLE_STATE;
    public State CHASE_STATE;
    public State GLOBAL_STATE;

    [Header("Each Controller Property")]
    [SerializeField] public Detector detector;


    private void Start()
    {
        Init();
        ChangeState(IDLE_STATE);        // IDLE 상태가 되기 전에 this로 controller의 속성을 전달한다. -> curState가 null인 상황으로 전달
                                        // new NormalMonsterStates.IdleState(this) 이렇게 사용해야 하나?
                                        // State의 Initialize 또는 ChangeState()를 만들어서 여기서 계속 controller의 속성을 세팅할건가
    }

    protected override void Init()
    {
        _machine = new StateMachine();
        IDLE_STATE = new NormalMonsterStates.IdleState(this);
        CHASE_STATE = new NormalMonsterStates.ChaseState(this);
        GLOBAL_STATE = new NormalMonsterStates.GlobalState(this);

        agent.stoppingDistance = detector.attackRange;
    }

    private void Update()
    {
        _machine.Execute();
    }
}
