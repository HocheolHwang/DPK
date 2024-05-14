using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dungeon_Popup_UI -> CurrentCheckPointIndex에서 현재 wave 정보를 확인할 수 있다
public abstract class Pattern : MonoBehaviour ,IPattern
{
    [Header("애니메이션이 재생된 이후 이펙트와 Hitbox가 생기는 시점 - 디버깅")]
    [SerializeField] protected float _createTime;
    [SerializeField] protected Vector3 _patternRange;

    [Header(" attackDMG + patternDMG ")]
    [SerializeField] protected int _attackDamage;
    [SerializeField] protected int _patternDmg;

    protected BaseMonsterController _controller;
    //protected int _curStage;                    // 무한의 탑 컨텐츠가 있는 경우 사용
    private Coroutine _currentCoroutine;

    protected HitBox _hitbox;
    protected ParticleSystem _ps;

    public BaseMonsterController Controller { get => _controller; private set => _controller = value; }
    public int AttackDamage { get => _attackDamage; private set => _attackDamage = value; }
    public Vector3 PatternRange { get => _patternRange; protected set => _patternRange = value; }
    public float CreateTime { get => _createTime; protected set => _createTime = value; }

    private void Start()
    {
        Init();
    }

    // --------------------------- Init ------------------------------
    public virtual void Init()
    {
        _controller = GetComponent<BaseMonsterController>();
        _attackDamage = _controller.Stat.AttackDamage;
    }

    // --------------------------- Pattern Logic ------------------------------
    public void Cast()
    {
        _currentCoroutine = StartCoroutine(StartPatternCast());
    }

    public void StopCast()
    {
        if (_currentCoroutine == null) return;
        StopCoroutine(_currentCoroutine);

        // Coroutine에서 생성한 게임 오브젝트 제거
        //if (_hitbox != null) Managers.Resource.Destroy(_hitbox.gameObject);
        //if (_ps != null) Managers.Effect.Stop(_ps);
    }

    public abstract IEnumerator StartPatternCast();

    public Vector3 DirectionToTarget(Vector3 curPos)
    {
        Vector3 targetPos = _controller.Detector.Target.position;
        Vector3 dirToTarget = targetPos - curPos;
        return dirToTarget.normalized;
    }
}
