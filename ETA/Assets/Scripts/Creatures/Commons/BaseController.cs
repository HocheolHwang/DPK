using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseController : MonoBehaviour, IDamageable
{
    protected StateMachine _stateMachine;
    protected State _curState;

    [Header("Common Property")]
    [SerializeField] public Define.UnitType UnitType;
    [SerializeField] public Animator Animator;
    [SerializeField] public NavMeshAgent Agent;
    [SerializeField] public IDetector Detector;
    [SerializeField] public Stat Stat;


    public StateMachine StateMachine { get => _stateMachine; set => _stateMachine = value; }
    public State CurState { get => _curState; set => _curState = _stateMachine.CurState; }


    float _changedColorTime = 0.1f;
    private Renderer[] _allRenderers; // 캐릭터의 모든 Renderer 컴포넌트
    private Color[] _originalColors;  // 원래의 머티리얼 색상 저장용 배열
    Color _damagedColor = Color.gray;

    float _destroyedTime = 3.0f;

    //-----------------------------------  Essential Functions --------------------------------------------
    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        Detector = GetComponent<IDetector>();
        Stat = GetComponent<Stat>();

        SetOriginColor();

        Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }
    protected virtual void Update()
    {
        _stateMachine.Execute();
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
    public virtual void TakeDamage(int attackDamage)
    {
        // 최소 데미지 = 1
        int damage = Mathf.Abs(attackDamage - Stat.Defense);
        if (damage <= 1)
        {
            damage = 1;
        }

        StartCoroutine(ChangeDamagedColorTemporarily());

        UI_AttackedDamage attackedDamage_ui = Managers.UI.MakeWorldSpaceUI<UI_AttackedDamage>(transform);
        attackedDamage_ui.AttackedDamage = damage;
        
        Stat.Hp -= damage;

        Debug.Log($"{gameObject.name} has taken {damage} damage.");
        if (Stat.Hp <= 0)
        {
            Stat.Hp = 0;
            DestroyObject();
        }
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
}
