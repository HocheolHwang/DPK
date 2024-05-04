using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public abstract class BaseController : MonoBehaviour, IDamageable
{
    protected StateMachine _stateMachine;
    protected State _curState;
    protected State _prevState;

    [Header("Common Property")]
    [SerializeField] public Define.UnitType UnitType;
    [SerializeField] public Animator Animator;
    [SerializeField] public NavMeshAgent Agent;
    [SerializeField] public IDetector Detector;
    [SerializeField] public Stat Stat;


    public StateMachine StateMachine { get => _stateMachine; set => _stateMachine = value; }
    public State CurState { get => _stateMachine.CurState; }
    public State PrevState { get => _stateMachine.PrevState; }

    float _changedColorTime = 0.15f;
    private Renderer[] _allRenderers; // 캐릭터의 모든 Renderer 컴포넌트
    private Color[] _originalColors;  // 원래의 머티리얼 색상 저장용 배열
    Color _damagedColor = Color.gray;
    protected ParticleSystem _shieldEffect;

    float _destroyedTime = 3.0f;

    //-----------------------------------  Essential Functions --------------------------------------------
    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        Detector = GetComponent<IDetector>();
        Stat = GetComponent<Stat>();

        // Stat 세팅
        Agent.speed = Stat.MoveSpeed;

        SetOriginColor();

        Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);

        StartCoroutine(TestDie());    // 수동으로 HP를 0으로 세팅해서 DIE EVENT를 확인
    }
    protected virtual void Update()
    {
        _stateMachine.Execute();

        if (Stat.Shield > 0 && _shieldEffect == null)
        {
            _shieldEffect = Managers.Resource.Instantiate("Effect/Shield", transform).GetComponent<ParticleSystem>();
            _shieldEffect.transform.localPosition += transform.up;
        }
        else
        {
            if(Stat.Shield <= 0 && _shieldEffect != null)
            {
                Managers.Resource.Destroy(_shieldEffect.gameObject);

            }
        }
    }
    protected abstract void Init();

    //----------------------------------- State Machine Functions --------------------------------------------
    public void ChangeState(State newState, bool forceReset = false)
    {
        // controller의 curState를 계속 갱신할 수 있다.
        _curState = newState;
        _stateMachine.ChangeState(newState, forceReset);
    }
    public void RevertToPrevState()
    {
        _curState = _stateMachine.PrevState;
        _stateMachine.RevertToPrevState();
    }

    //----------------------------------- Debugging --------------------------------------------
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (_stateMachine != null && _curState != null)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;


            string label = "Active State: " + _curState.ToString();
            Handles.Label(transform.position, label, style);
        }
#endif
    }

    // ---------------------------------- IDamage ------------------------------------------
    public virtual void TakeDamage(int attackDamage, bool isCounter = false)
    {
        // 최소 데미지 = 1
        int damage = attackDamage - Stat.Defense;
        if (damage <= 1)
        {
            damage = 1;
        }

        UI_AttackedDamage attackedDamage_ui = null;

        if (Stat.Shield >= damage)
        {
            Stat.Shield -= damage;
            attackedDamage_ui = Managers.UI.MakeWorldSpaceUI<UI_AttackedDamage>(transform);
            attackedDamage_ui.IsGurared = true;
            return;
        }
        else
        {
            damage -= Stat.Shield;
            Stat.Shield = 0;
        }

        StartCoroutine(ChangeDamagedColorTemporarily());

        attackedDamage_ui = Managers.UI.MakeWorldSpaceUI<UI_AttackedDamage>(transform);
        attackedDamage_ui.AttackedDamage = damage;
        
        Stat.Hp -= damage;
        AttackedEvent();

        Debug.Log($"{gameObject.name} has taken {damage} damage.");
        if (Stat.Hp <= 0)
        {
            Stat.Hp = 0;
            DestroyObject();
        }
        else if (isCounter)
        {
            CounterEvent();
        }
    }

    public virtual void AttackedEvent() {
        
    }

    public virtual void CounterEvent()
    {
        // 카운터, 히든 기믹 파훼 판단
    }

    public virtual void DestroyEvent()
    {
        // 파괴, 이펙트, 소리, UI 등 다양한 이벤트 추가
        // 관련 Resource는 Component나 Manager로 가져옴
        Debug.Log("Destory Event Start");

        // 애니메이션은 상태에서 관리 중
        GetComponent<Collider>().enabled = false;


    }

    public virtual void DestroyObject()
    {
        DestroyEvent();
        Destroy(gameObject, _destroyedTime);
    }


    void SetOriginColor()
    {
        _allRenderers = GetComponentsInChildren<Renderer>();
        _originalColors = new Color[_allRenderers.Length];

        // 각 Renderer의 원래 머티리얼 색상 저장
        for (int i = 0; i < _allRenderers.Length; i++)
        {
            _originalColors[i] = _allRenderers[i].material.color;
        }
    }

    IEnumerator ChangeDamagedColorTemporarily()
    {
        foreach (Renderer renderer in _allRenderers)
        {
            renderer.material.SetColor("_Color", _damagedColor);
            renderer.material.SetColor("_BaseColor", _damagedColor);
        }
        // 지정된 시간만큼 기다림
        yield return new WaitForSeconds(_changedColorTime);

        // 모든 Renderer의 머티리얼 색상을 원래 색상으로 복구
        for (int i = 0; i < _allRenderers.Length; i++)
        {
            Renderer renderer = _allRenderers[i];
            renderer.material.SetColor("_BaseColor", Color.white);
            renderer.material.color = _originalColors[i];
        }
    }

    public void GetShield(int amount)
    {
        Stat.Shield += amount;
    }

    public void RemoveShield(int amount)
    {
        Stat.Shield -= amount;
        if (Stat.Shield < 0) Stat.Shield = 0;
    }


    // TakeDamage를 통해서만 DestoryObject를 수행할 수 있기 때문에 TEST를 위한 함수 추가
    IEnumerator TestDie()
    {
        Debug.Log($"------------- TEST DIE --------------");
        Debug.Log($"------------- TEST SUMMON SKILL - DespawnAll --------------");

        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (Stat.Hp <= 0)
            {
                TakeDamage(Stat.MaxHp + Stat.Defense);
                yield break;
            }
        }
        
        
    }
}
