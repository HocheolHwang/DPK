using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseController : MonoBehaviour, IDamageable
{
    protected StateMachine _stateMachine;
    protected State _curState;
    protected Stat _stat;

    [Header("Common Property")]
    [SerializeField] public Define.UnitType unitType;
    [SerializeField] public Animator animator;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public Detector detector;


    public StateMachine StateMachine { get => _stateMachine; set => _stateMachine = value; }
    public State CurState { get => _curState; set => _curState = _stateMachine.CurState; }
    public Stat Stat { get => _stat; set => _stat = value; }


    //-----------------------------------  Essential Functions --------------------------------------------
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        detector = GetComponent<Detector>();
    }
    protected virtual void Update()
    {
        _stateMachine.Execute();
    }
    protected abstract void Init();

    //----------------------------------- State Machine Functions --------------------------------------------
    public void ChangeState(State newState)
    {
        // controller의 curState를 계속 갱신할 수 있다.
        _curState = newState;
        _stateMachine.ChangeState(newState);
    }
    public void RevertToPrevState()
    {
        _curState = _stateMachine.PrevState;
        _stateMachine.RevertToPrevState();
    }

    //----------------------------------- Debugging --------------------------------------------
    private void OnDrawGizmos()
    {

        if (_stateMachine != null && _curState != null)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;

            string label = "Active State: " + _curState.ToString();
            Handles.Label(transform.position, label, style);
        }
    }

    // ---------------------------------- Detector ------------------------------------------
    public bool IsArriveToTarget()
    {
        return Vector3.Distance(detector.Target.position, transform.position) < detector.attackRange;
    }

    // ---------------------------------- IDamage ------------------------------------------
    public virtual void TakeDamage(int attackDamage)
    {
        // 최소 데미지 = 1
        int damage = Mathf.Abs(attackDamage - _stat.Defense);
        if (damage <= 1)
        {
            damage = 1;
        }
        _stat.Hp -= damage;

        Debug.Log($"{gameObject.name} has taken {damage} damage.");
        if (_stat.Hp <= 0)
        {
            _stat.Hp = 0;
            DestroyObject();
        }
    }

    public virtual void DestroyEvent()
    {
        // 파괴, 이펙트, 소리, UI 등 다양한 이벤트 추가
        // 관련 Resource는 Component나 Manager로 가져옴

        // 애니메이션은 상태에서 관리 중
    }

    public virtual void DestroyObject()
    {
        DestroyEvent();
        Destroy(gameObject, 0.5f);
    }
}
