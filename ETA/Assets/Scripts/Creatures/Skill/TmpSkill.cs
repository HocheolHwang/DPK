using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TmpSkill : MonoBehaviour
{
    protected PlayerController _controller;
    protected Animator _animator;
    protected SkillSystem _skillSystem;
    protected float _lastExcuteTime;
    protected float _cooldownTime;

    public Define.SkillType SkillType = Define.SkillType.Target;
    public int Damage = 50;
    public Vector3 skillRange;

    private Coroutine _currentCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        _controller = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
        _skillSystem = GetComponent<SkillSystem>();
        
    }

    public void SetCoolDownTime(float time)
    {
        _cooldownTime = time;
        _lastExcuteTime = _cooldownTime * -1;
    }


    public void Cast()
    {
        _lastExcuteTime = Time.time;
        _currentCoroutine = StartCoroutine(StartSkillCast());
    }

    public void CollavoCast()
    {
        _lastExcuteTime = Time.time;
        _currentCoroutine = StartCoroutine(StartCollavoSkillCast());
    }

    public bool CanCastSkill()
    {
        if (Time.time - _lastExcuteTime >= _cooldownTime) return true;
        return false;
    }

    public abstract IEnumerator StartSkillCast();
    public virtual IEnumerator StartCollavoSkillCast() {
        yield return null;
    }

    public void StopCast()
    {
        if (_currentCoroutine == null) return;
        StopCoroutine(_currentCoroutine);
    }


}
