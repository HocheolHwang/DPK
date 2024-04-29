using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class KnightGCounterEnablePattern : Pattern
{
    KnightGAnimationData _animData;
    KnightGController _kcontroller;

    [Header("원하는 이펙트와 HIT_COUNTER 소리 이름을 넣으세요 - 디버깅")]
    [SerializeField] string _effectName;
    [SerializeField] string _soundName;
    

    [Header("개발 편의성")]
    [SerializeField] Vector3 _hitboxRange = new Vector3(2.0f, 4.0f, 2.0f);
    [SerializeField] float _upLoc = 2.0f;

    private float _duration;

    public override void Init()
    {
        base.Init();
        _animData = _controller.GetComponent<KnightGAnimationData>();
        _kcontroller = _controller.GetComponent<KnightGController>();
        _duration = _animData.CounterEnableAnim.length * 4.0f - _createTime;

        _createTime = 0.1f;
        _patternRange = _hitboxRange;
    }

    public override IEnumerator StartPatternCast()
    {
        // 멈췄을 때 target을 향해 hitbox, effect 생성
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upLoc);
        Vector3 objectLoc = transform.position + rootUp;

        yield return new WaitForSeconds(_createTime);

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        //ParticleSystem ps = Managers.Resource.Instantiate($"Effect/{_effectName}").GetComponent<ParticleSystem>();
        hitbox.SetUp(transform, _attackDamage);
        hitbox.transform.localScale = _patternRange;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.position = objectLoc;

        // 시전 도중에 카운터 스킬을 맞으면 hit box와 effect가 사라지고, sound가 발생
        float timer = 0;
        while (timer < _duration)
        {
            if (_kcontroller.IsHitCounter)
            {
                Managers.Resource.Destroy(hitbox.gameObject);
                //Managers.Resource.Destroy(ps.gameObject);
                // 소리 발생
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Managers.Resource.Destroy(hitbox.gameObject);
        //Managers.Resource.Destroy(ps.gameObject);
    }
}
