using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Boar의 근거리 공격
public class BoarAutoAttack : Pattern
{
    [Header("원하는 이펙트 이름을 넣으세요 - 디버깅")]
    [SerializeField] string _effectName;

    [Header("개발 편의성")]
    [SerializeField] float _hitboxForwardLoc = 0.7f;
    [SerializeField] Vector3 _hitboxRange = new Vector3(0.4f, 0.5f, 1.3f);
    [SerializeField] float _upLoc = 0.4f;

    public override void Init()
    {
        base.Init();

        _createTime = 0.1f;

        _patternRange = _hitboxRange;
        if (gameObject.name == "BoarBoss")
        {
            _patternRange *= 2;
        }
    }

    public override IEnumerator StartPatternCast()
    {
        // 멈췄을 때 target을 향해 hitbox, effect 생성
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * (_controller.Detector.AttackRange - _hitboxForwardLoc)); // Target이 null일 수 있기 때문에 임의로 지정
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upLoc);
        Vector3 objectLoc = transform.position + rootForward + rootUp;
        //Vector3 dir = _controller.Detector.Target.position - transform.position;
        //Vector3 _objectPosition = transform.position + dir.normalized * _controller.Detector.AttackRange;

        yield return new WaitForSeconds(_createTime);

        Managers.Sound.Play("Monster/Boar/BoarAttack_SND", Define.Sound.Effect);

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        //ParticleSystem ps = Managers.Resource.Instantiate($"Effect/{_effectName}").GetComponent<ParticleSystem>();
        hitbox.SetUp(transform, _attackDamage);
        hitbox.transform.localScale = _patternRange;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.position = objectLoc;
        
        //ps.transform.position = hitbox.transform.position;

        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);

        //yield return new WaitForSeconds(ps.main.duration);
        //Managers.Resource.Destroy(ps.gameObject);
    }
}
